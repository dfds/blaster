using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.MyServices;
using Blaster.WebApi.Features.Namespaces;
using Blaster.WebApi.Features.System;
using Blaster.WebApi.Features.Teams;
using Blaster.WebApi.Security;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using k8s;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Prometheus;

namespace Blaster.WebApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            /* configure generic functionality */
            ConfigureMvc(services);
            ConfigureAuthentication(services);
            services.AddTransient<ForwardedHeaderBasePath>();
            services.AddHttpContextAccessor();
            services.AddTransient<IdTokenAccessor>();
            services.AddCorrelationId();
            services.AddTransient<CorrelationIdMessageHandler>();
            services.AddTransient<IJsonSerializer, JsonSerializer>();

            /* configure each feature */
            ConfigureNamespacesFeature(services);
            ConfigureTeamsFeature(services);
            ConfigureDashboardsFeature(services);
            ConfigureMyServicesFeature(services);
        }

        private void ConfigureMyServicesFeature(IServiceCollection services)
        {
            services
                .AddHttpClient<IUserServicesService, UserServicesService>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BLASTER_TEAMSERVICE_API_URL"]);
                })
                .AddHttpMessageHandler<CorrelationIdMessageHandler>();

            services
                .AddHttpClient<ICognitoService, CognitoService>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BLASTER_TEAMSERVICE_API_URL"]);
                })
                .AddHttpMessageHandler<CorrelationIdMessageHandler>();
        }

        private void ConfigureDashboardsFeature(IServiceCollection services)
        {
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddSingleton<ExternalDashboardServiceSettings>(serviceProvider => new ExternalDashboardServiceSettings
            {
                ServiceEndpoint = Configuration["BLASTER_DASHBOARD_SERVICE_URL"]
            });
        }

        private void ConfigureTeamsFeature(IServiceCollection services)
        {
            services
                .AddHttpClient<ITeamService, TeamService>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BLASTER_TEAMSERVICE_API_URL"]);
                })
                .AddHttpMessageHandler<CorrelationIdMessageHandler>();
        }

        private void ConfigureNamespacesFeature(IServiceCollection services)
        {
            services.AddTransient<IKubernetes>(serviceProvider =>
            {
                var config = _env.IsDevelopment()
                    ? KubernetesClientConfiguration.BuildConfigFromConfigFile()
                    : KubernetesClientConfiguration.InClusterConfig();

                return new Kubernetes(config);
            });

            services.AddTransient<INamespaceRepository, NamespaceRepository>();
        }

        protected virtual void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    if (!_env.IsDevelopment())
                    {
                        var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();

                        options.Filters.Add(new AuthorizeFilter(policy));
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new FeatureLocationExpander());
            });
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
//                .AddCookie(options =>
//                {
//                    // What default values from AddCookie() needs to be added here?
//                    options.Events = new CookieAuthenticationEvents
//                    {
//                        OnValidatePrincipal = RefreshAwsTokenIfNeeded
//                    };
//                })
                .AddOpenIdConnect(options =>
                {
                    var poolId = Configuration["BLASTER_COGNITO_POOL_ID"];
                    var region = Configuration["BLASTER_COGNITO_REGION"];
                    var clientId = Configuration["BLASTER_COGNITO_CLIENT_ID"];
                    var clientSecret = Configuration["BLASTER_COGNITO_CLIENT_SECRET"];

                    options.ResponseType = "code";
                    options.MetadataAddress =
                        $"https://cognito-idp.{region}.amazonaws.com/{poolId}/.well-known/openid-configuration";
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        NameClaimType = "name"
                    };
                    options.SaveTokens = true;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseForwardedHeadersAsBasePath();
            app.UseMetricServer();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCorrelationId(new CorrelationIdOptions
            {
                Header = "x-correlation-id",
                UpdateTraceIdentifier = true,
                IncludeInResponse = true
            });

            app.UseMvc();
        }
    }

    public class ForwardedHeaderBasePath : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
            {
                //context.Request.Path = prefix + context.Request.Path;
                context.Request.PathBase = new PathString(prefix);
            }

            if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var protocol))
            {
                context.Request.Scheme = protocol;
            }

            await next(context);
        }
    }

    public static class ForwardedHeadersAsBasePathExtensions
    {
        public static IApplicationBuilder UseForwardedHeadersAsBasePath(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ForwardedHeaderBasePath>();
        }
    }

    public class CorrelationIdMessageHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CorrelationIdMessageHandler(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headerName = _correlationContextAccessor.CorrelationContext.Header;
            var correlationId = _correlationContextAccessor.CorrelationContext.CorrelationId;

            if (!request.Headers.Contains(headerName))
            {
                request.Headers.Add(headerName, correlationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
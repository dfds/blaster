using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Namespaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using k8s;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(_env);
            
            services.AddTransient<IKubernetes>(serviceProvider =>
            {
                var config = _env.IsDevelopment()
                    ? KubernetesClientConfiguration.BuildConfigFromConfigFile()
                    : KubernetesClientConfiguration.InClusterConfig();

                return new Kubernetes(config);
            });

            services.AddTransient<IApiKeyValidator, EnvironmentVariableBasedApiKeyValidator>();
            services.AddTransient<INamespaceRepository, NamespaceRepository>();
            services.AddSingleton<HttpClient>();
            services.AddTransient<IJsonSerializer, JsonSerializer>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddSingleton<ExternalDashboardServiceSettings>(serviceProvider => new ExternalDashboardServiceSettings
            {
                ServiceEndpoint = Configuration["BLASTER_DASHBOARD_SERVICE_URL"]
            });

            services
                .AddAuthentication(options =>
                {
                    //options.DefaultAuthenticateScheme = "dfds-apikey";
                    options.DefaultScheme = "dfds-apikey";

                    //options.AddScheme<CustomApiAuthenticationHandler>("dfds-apikey", "DFDS API Key");
                }).AddScheme<CustomApiOptions, CustomApiAuthenticationHandler>("dfds-apikey", options =>
                {
                    
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

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

            app.UseMetricServer();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }

    public class CustomApiAuthenticationHandler : AuthenticationHandler<CustomApiOptions>
    {
        private readonly IApiKeyValidator _apiKeyValidator;

        public CustomApiAuthenticationHandler(IOptionsMonitor<CustomApiOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyValidator apiKeyValidator) 
            : base(options, logger, encoder, clock)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (this.Request.Headers.TryGetValue("x-dfds-apikey", out var value))
            {
                var isValid = await _apiKeyValidator.IsValid(value.ToString());

                if (isValid)
                {
                    var identity = new ClaimsIdentity();
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }

            return AuthenticateResult.Fail("nooooooo");
        }
    }

    public class CustomApiOptions : AuthenticationSchemeOptions
    {
        
    }

    public static class MvcConfigurationExtensions
    {
        public static IServiceCollection AddMvc(this IServiceCollection services, IHostingEnvironment env)
        {
            services
                .AddMvc(options =>
                {
                    //if (!env.IsDevelopment())
                    //{
                    //    options.Filters.Add<ApiKeyFilter>();
                    //}

                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new FeatureLocationExpander());
            });


            return services;
        }
    }
}

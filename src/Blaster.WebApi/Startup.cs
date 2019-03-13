using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blaster.WebApi.Features.Capabilities;
using Blaster.WebApi.Features.Frontpage;
using Blaster.WebApi.Features.Topic;
using Blaster.WebApi.Security;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Prometheus;

namespace Blaster.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
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
            services.AddTransient<JsonSerializer>();
            services.AddTransient<UserHelper>();

            /* configure each feature */
            ConfigureCapabilityFeature(services);
            ConfigureFrontpageFeature(services);
            ConfigureTopicFeature(services);
        }

        protected virtual void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new FeatureLocationExpander());
            });
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            // ...
        }

        private void ConfigureCapabilityFeature(IServiceCollection services)
        {
            services
                .AddHttpClient<ICapabilityServiceClient, CapabilityServiceClient>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BLASTER_CAPABILITYSERVICE_API_URL"]);
                })
                .AddHttpMessageHandler<CorrelationIdMessageHandler>();
        }

        private void ConfigureFrontpageFeature(IServiceCollection services)
        {
            services
                .AddHttpClient<IIamRoleService, IamRoleService>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BLASTER_IAMROLESERVICE_API_URL"]);
                })
                .AddHttpMessageHandler<CorrelationIdMessageHandler>();
        }

        private void ConfigureTopicFeature(IServiceCollection services)
        {
            services
                .AddHttpClient<ITopicClient, TikaTopicClient>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BLASTER_TIKA_API_URL"]);
                })
                .AddHttpMessageHandler<CorrelationIdMessageHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeadersAsBasePath();
            app.UseMetricServer();
            app.UseStaticFiles();
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
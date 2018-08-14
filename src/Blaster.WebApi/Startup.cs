using System.Collections.Generic;
using Blaster.WebApi.Features.Dashboards;
using Blaster.WebApi.Features.Namespaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using k8s;
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

            services.AddTransient<INamespaceRepository, NamespaceRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
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
            app.UseMvc();
        }
    }

    public static class MvcConfigurationExtensions
    {
        public static IServiceCollection AddMvc(this IServiceCollection services, IHostingEnvironment env)
        {
            services
                .AddMvc(options =>
                {
                    if (!env.IsDevelopment())
                    {
                        options.Filters.Add<ApiKeyFilter>();
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new FeatureLocationExpander());
            });

            services.AddTransient<IApiKeyValidator, EnvironmentVariableBasedApiKeyValidator>();

            return services;
        }
    }

    public class FeatureLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Features/{1}/{0}.cshtml",
                "/Features/{1}s/{0}.cshtml",
                "/Features/Shared/{0}.cshtml"
            };
        }
    }
}

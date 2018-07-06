using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using k8s;

namespace Blaster.WebApi
{
    public class Startup
    {
        public static KubernetesClientConfiguration kubeConfig;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    //options.Filters.Add<ApiKeyFilter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddTransient<IApiKeyValidator, EnvironmentVariableBasedApiKeyValidator>();
            
            
            services.AddTransient<IKubernetes>(k => new Kubernetes(kubeConfig));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            }
            else
            {
                app.UseHsts();
                kubeConfig = KubernetesClientConfiguration.InClusterConfig();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

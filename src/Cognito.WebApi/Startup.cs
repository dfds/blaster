using Cognito.WebApi.Controllers;
using Cognito.WebApi.Model;
using Cognito.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Cognito.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register the Swagger generator, defining 1 or more Swagger documents
            var apiVersion = "v1";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(apiVersion, new Info {Title = "Cognito API", Version = apiVersion});
            });

            ConfigureDependencyInjectionContainer(services);
        }

        public void ConfigureDependencyInjectionContainer(IServiceCollection services)
        {
            services.AddTransient<IAwsConsoleLinkBuilder, AwsConsoleLinkBuilder>();
            var variables = new Variables();
            variables.Validate();
            services.AddSingleton<IVariables>(variables);

            services.AddTransient<UserPoolClient>((s) =>
            {
                var vars = s.GetRequiredService<IVariables>();


                return new UserPoolClient(
                    vars.AwsCognitoAccessAccessKey,
                    vars.AwsCognitoSecretAccessKey,
                    vars.AwsCognitoUserPoolId);
            });

            services.AddTransient<TeamsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cognito API"); });

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionMiddleware().Invoke
            });

            app.UseMvc();
        }
    }
}
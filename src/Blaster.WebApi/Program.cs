using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace Blaster.WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
	        var loggerConfiguration = new LoggerConfiguration();
	        ApplyDefaultLoggingSetupTo(loggerConfiguration);

	        Log.Logger = loggerConfiguration
		        .WriteTo.Console(new CompactJsonFormatter())
		        .CreateLogger();

			try
            {
                Log.Information("Starting host");
                CreateWebHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ApplyDefaultLoggingSetupTo(LoggerConfiguration loggerConfiguration)
        {
	        loggerConfiguration
			        .Enrich.FromLogContext()
			        .MinimumLevel.Information()
			        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
		        ;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
	        return WebHost
		        .CreateDefaultBuilder(args)
		        .UseSerilog((context, configuration) =>
		        {
			        ApplyDefaultLoggingSetupTo(configuration);
			        if (context.HostingEnvironment.IsDevelopment())
			        {
				        configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
			        }
		        })
		        .UseContentRoot(Directory.GetCurrentDirectory())
		        .UseStartup<Startup>();
        }
    }
}

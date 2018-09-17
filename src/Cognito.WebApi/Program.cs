using System;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Cognito.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            Console.WriteLine($"prcess id: {Process.GetCurrentProcess().Id}");
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config =>
                    {
                        IConfiguration settings = config.Build();
                        string connectionString = settings.GetValue<string>("AppConfig");

                        // Load configuration from Azure App Configuration
                        config.AddAzureAppConfiguration(connectionString);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}

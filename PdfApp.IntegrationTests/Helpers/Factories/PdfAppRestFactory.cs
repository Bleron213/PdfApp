using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.IntegrationTests.Helpers.Factories
{
    public class PdfAppRestFactory : WebApplicationFactory<Program>
    {
        public override IServiceProvider Services => base.Services;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(configHost =>
            {
                configHost.AddEnvironmentVariables();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                var envType = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            
                config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{envType}.json", false, true)
                    .Build();
            });

            return base.CreateHost(builder);
        }

        //protected override IHostBuilder? CreateHostBuilder()
        //{
            //return Host.CreateDefaultBuilder()
            //    .ConfigureWebHost(builder =>
            //    {
            //        builder.UseStartup<Program>();
            //    })
            //     .ConfigureHostConfiguration(configHost =>
            //     {
            //         configHost.AddEnvironmentVariables();
            //     })
            //     .ConfigureAppConfiguration((context, config) =>
            //     {
            //         var envType = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            //         config
            //             .SetBasePath(Directory.GetCurrentDirectory())
            //             .AddJsonFile("appsettings.json", false, true)
            //             .AddJsonFile($"appsettings.{envType}.json", false, true)
            //             .Build();
            //     });
    }
}

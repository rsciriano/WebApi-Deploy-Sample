using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((builderContext, config) =>
                {                    
                    // Load secrets general appsettings mapped as Docker secrets file
                    config.AddJsonFile("/run/secrets/appsettings.secrets.json", true, true);

                    // Load secrets specific enviroiment appsettings mapped as Docker secrets file
                    config.AddJsonFile($"/run/secrets/appsettings.{builderContext.HostingEnvironment.EnvironmentName}.secrets.json", true, true);
                });
    }
}

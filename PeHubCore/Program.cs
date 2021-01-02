using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceExt;

namespace PeHubCore
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
                    webBuilder.UseStartup<Startup>();
                    if (Appsettings.GetSectionValue("AppSettings:isDocker") != "1")
                    {
                        if (!string.IsNullOrEmpty(Appsettings.GetSectionValue("AppSettings:RunUrl")))
                        {
                            webBuilder.UseUrls(Appsettings.GetSectionValue("AppSettings:RunUrl").Split(','));
                        }
                    }
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}

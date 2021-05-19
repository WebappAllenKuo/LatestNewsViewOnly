using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Infra.Extensions;
using NLog.Web;

namespace LatestNewsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var processPath    = AppDomain.CurrentDomain.BaseDirectory;
            var nlogConfigPath = Path.Combine(processPath, "3rd Party", "NLog", "nlog.config");
            var log            = NLogBuilder.ConfigureNLog(nlogConfigPath).GetCurrentClassLogger();

            Host.CreateDefaultBuilder(args)
                .UseNLog()
                .ConfigureAppConfiguration((hostingContext, config) => config.AddJsonFiles())
                .ConfigureWebHostDefaults(webBuilder =>
                                          {
                                              webBuilder.UseStartup<Startup>();
                                          })
                .Build()
                .Run();
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App.Utilities
{
    public class Utils
    {
        /// <summary>
        /// Get config
        /// </summary>
        /// <param name="env">Môi trường chạy</param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            return builder.Build();
        }
    }
}

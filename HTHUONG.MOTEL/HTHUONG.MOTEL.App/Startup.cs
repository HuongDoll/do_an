using HTHUONG.MOTEL.App.Extensions;
using HTHUONG.MOTEL.App.Utilities;
using HTHUONG.MOTEL.Core.Utils;
using HTHUONG.MOTEL.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTHUONG.MOTEL.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        readonly string AllowdOrigins = "AllowdOrigins";

        public Startup(IWebHostEnvironment env)
        {
            this.Environment = env;
            this.Configuration = Utils.GetConfiguration(env);
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SysConfig.configuration = Configuration;
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowdOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                                  });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HTHUONG.MOTEL.App", Version = "v1" });
            });
            DapperDatabaseContext.ConnectionString = CommonFunction.GetAppSettings("DatabaseConfig", "ConnectionString");
            DapperDatabaseContext.DatabaseName = CommonFunction.GetAppSettings("DatabaseConfig", "DatabaseName");
            services.AddLifetimeServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTHUONG.MOTEL.App v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowdOrigins");

            app.UseAuthorization();

            app.UseMiddleware<AuthenticationMiddleWare>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

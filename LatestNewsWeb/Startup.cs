using System;
using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LatestNewsWeb.Infra;
using LatestNewsWeb.Parameters;
using LatestNewsWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Services;

namespace LatestNewsWeb
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
            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddServices();

            services.AddScoped<Infra.Validate.ValidatorFactory>();
            services.AddScoped<Infra.Validate.News.ValidatorDto>();
            services.AddScoped<Infra.Validate.ValidatorPageInfoDto>();

            services.AddScoped<FileService>();
            services.AddScoped<FileUploadService>();

            services.AddTransient<IDbConnection>(provider =>
                                                 {
                                                     var connectionString = Configuration.GetConnectionString("DefaultConnection");
                                                     return new SqlConnection(connectionString);
                                                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapAreaControllerRoute(
                                                                  name: "BackStageAreas",
                                                                  areaName: "BackStage",
                                                                  pattern: "BackStage/{controller=Home}/{action=Index}/{guid?}");

                                 endpoints.MapControllerRoute(
                                                              name: "default",
                                                              pattern: "{controller=Home}/{action=Index}/{guid?}");
                             });
        }
    }
}

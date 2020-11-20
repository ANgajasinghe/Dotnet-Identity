using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration  configuration)
        {
            _config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<AppDbContext>(config =>
            //{
            //    config.UseInMemoryDatabase("Memory");
            //});



            services.AddDbContext<AppDbContext>(options =>
              options.UseSqlServer(_config.GetConnectionString("DBConnection")));

            // This code block will regeister the identity service in the run-time 
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    // Set custom password rules
                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequiredUniqueChars = 0;
                    config.Password.RequiredLength = 0;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                    config.SignIn.RequireConfirmedEmail = true;
                 
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // In this code block works with the identity system and override setting as bolow
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Nayanajith.Cookie";
                config.LoginPath = "/Home/Login";
            });


            // Set up mail client 
            var mailKitOption = _config.GetSection("STMP").Get<MailKitOptions>();
            services.AddMailKit(config => config.UseMailKit(mailKitOption));


            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();



            // who are you?
            app.UseAuthentication();

            // Are You Allowed?
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

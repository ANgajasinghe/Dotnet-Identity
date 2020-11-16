using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basic
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            // This is Cookie handler
            // Tells application to who we are 
            // Inject to the app.UseAuthentication(); method
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth" , configureOptions=> 
                {
                    configureOptions.Cookie.Name = "Nayanajith.Cookie";
                    configureOptions.LoginPath = "/Home/Autenticate";
                });

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

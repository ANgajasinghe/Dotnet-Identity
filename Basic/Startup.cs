using Basic.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

            // This Will Happend Under The Hood IN [Authorize] interface 
            services.AddAuthorization(config =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();

                //var defaultAuthPolicy = defaultAuthBuilder
                //    .RequireAuthenticatedUser() // Add all Authrntication stuff to your application
                //    .RequireClaim(ClaimTypes.DateOfBirth) // If this Claim not included your cookie you cannot visit the site
                //    .Build();

                //config.DefaultPolicy = defaultAuthPolicy;


                // ADDING CUSTOME REQUIRE CLAIM
                //config.AddPolicy("Claim.DoB", policyBuilder =>
                //{
                //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //});



                config.AddPolicy("AdminPolicy", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    // Custom Initialization with separate method BUT SAME IMPLEMETATION
                    policyBuilder.RequireCustomeClaim(ClaimTypes.DateOfBirth);
                });

            });



            services.AddScoped<IAuthorizationHandler, CustomeRequireClaimHandler>();

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

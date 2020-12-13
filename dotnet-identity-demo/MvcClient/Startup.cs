using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                // This will retrive the open id and it know how to communicate with the server
                // This will add profile scope and openId scope

                 // This Token not only for limited APIs But for all----
                .AddOpenIdConnect("oidc", config => {
                    config.Authority = "https://localhost:44377/";
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;

                    config.ResponseType = "code";
                   // config.SignedOutCallbackPath = "/Home/Index";

                    // configure cookie claim mapping 
                    // Givingr you to access to configuare your claims
                       config.ClaimActions.DeleteClaim("amr");
                       config.ClaimActions.DeleteClaim("s_hash");
                       config.ClaimActions.MapUniqueJsonKey("RawCoding.Grandma", "rc.garndma");


                    // two trips to load claims in to the cookie
                    // but the id token is smaller ! Or else id token is bigger 
                    config.GetClaimsFromUserInfoEndpoint = true;

                    // configure scope
                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    //config.Scope.Add("profile");
                    config.Scope.Add("rc.scope");
                    config.Scope.Add("ApiOne");
                    //config.Scope.Add("ApiTwo");

                    // this is for refresh token 
                    config.Scope.Add("offline_access");

                });

            services.AddHttpClient();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

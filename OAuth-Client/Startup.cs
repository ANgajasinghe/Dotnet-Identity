using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth_Client
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => {
                // We check cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                // When we sigh in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                // use this to check if we are allowed to do something.
                config.DefaultChallengeScheme = "OuerServer";

            })
                .AddCookie("ClientCookie")
                .AddOAuth("OuerServer", config => 
                {
                    // Come from the server
                    config.ClientId = "Client_Id";
                    config.ClientSecret = "Client_secrect";
                    config.CallbackPath = "/oauth/callback";
                    config.AuthorizationEndpoint = "https://localhost:44393/oauth/authorize";
                    config.TokenEndpoint = "https://localhost:44393/oauth/token";
                    config.SaveTokens = true;

                    config.Events = new OAuthEvents()
                    {
                        // Give access to the token and do stuff 
                        OnCreatingTicket = context => 
                        {
                            var accessToken = context.AccessToken;
                            var base64Payload = accessToken.Split('.')[1];
                            var bytes = Convert.FromBase64String(base64Payload);
                            var jasonPayload = Encoding.UTF8.GetString(bytes);
                            var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jasonPayload);

                            foreach (var claim in claims)
                            {
                                context.Identity.AddClaim(new System.Security.Claims.Claim(claim.Key, claim.Value));
                            }


                            return Task.CompletedTask;
                        }
                    };
                    
                });
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
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

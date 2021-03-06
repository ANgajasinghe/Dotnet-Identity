﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),

                // Uou need to add all Custome clames here or not cliwnt can not access that 

                // you have to add your claims here
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.garndma"
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("ApiOne"),
                
                //// more formal
                //new ApiScope("api.scope1"),
                //new ApiScope("api.scope2"),
                
                //// scope without a resource
                //new ApiScope("scope2"),
                
                //// policyserver
                //new ApiScope("policyserver.runtime"),
                //new ApiScope("policyserver.management")
            };
        }

        public static IEnumerable<ApiResource> GetApis() =>
            // Two typw of scopes have 1 = Api , 2 = Roal and Claims 
            new List<ApiResource> {
                new ApiResource("ApiOne",new string[]{"rc.api.garndma"})
                {
                    ApiSecrets =  { new Secret("client_secret".ToSha256()) },
                    // Set Scope for api
                    Scopes = { "ApiOne"}
                },
                new ApiResource("ApiTwo","Demo Two")
                {
                    ApiSecrets =  { new Secret("client_secret_mvc".ToSha256()) },
                    Scopes = {  "ApiOne","ApiTwo", IdentityServerConstants.StandardScopes.OpenId }
                },
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "ApiOne" }
                },
                new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    //https://localhost:44322/
                    RedirectUris = { "https://localhost:44322/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44322/Home/Index" },

                    AllowedScopes = {
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope",
                    },

                    // puts all the claims in the id token  and this can be bigger token other - THIS will lead us to two round trip and smaller token 
                    // AlwaysIncludeUserClaimsInIdToken = true,

                    // for refresh token 
                    AllowOfflineAccess = true,



                    // User feedback validation message screen enabale if you want when you creationg your application
                    RequireConsent = false,
                },
  //              new Client {
  //                  ClientId = "client_id_js",

  //                  AllowedGrantTypes = GrantTypes.Code,
  //                  RequirePkce = true,
  //                  RequireClientSecret = false,

  //                  RedirectUris = { "https://localhost:44345/home/signin" },
  //                  PostLogoutRedirectUris = { "https://localhost:44345/Home/Index" },
  //                  AllowedCorsOrigins = { "https://localhost:44345" },

  //                  AllowedScopes = {
  //                      IdentityServerConstants.StandardScopes.OpenId,
  //                      "ApiOne",
  //                      "ApiTwo",
  //                      "rc.scope",
  //                  },

  //                  AccessTokenLifetime = 1,

  //                  AllowAccessTokensViaBrowser = true,
  //                  RequireConsent = false,
  //              },

  //              new Client {
  //                  ClientId = "angular",

  //                  AllowedGrantTypes = GrantTypes.Code,
  //                  RequirePkce = true,
  //                  RequireClientSecret = false,

  //                  RedirectUris = { "http://localhost:4200" },
  //                  PostLogoutRedirectUris = { "http://localhost:4200" },
  //                  AllowedCorsOrigins = { "http://localhost:4200" },

  //                  AllowedScopes = {
  //                      IdentityServerConstants.StandardScopes.OpenId,
  //                      "ApiOne",
  //                  },

  //                  AllowAccessTokensViaBrowser = true,
  //                  RequireConsent = false,
  //              },

  //              new Client {
  //                  ClientId = "wpf",

  //                  AllowedGrantTypes = GrantTypes.Code,
  //                  RequirePkce = true,
  //                  RequireClientSecret = false,

  //                  RedirectUris = { "http://localhost/sample-wpf-app" },
  //                  AllowedCorsOrigins = { "http://localhost" },

  //                  AllowedScopes = {
  //                      IdentityServerConstants.StandardScopes.OpenId,
  //                      "ApiOne",
  //                  },

  //                  AllowAccessTokensViaBrowser = true,
  //                  RequireConsent = false,
  //              },
  //              new Client {
  //                  ClientId = "xamarin",

  //                  AllowedGrantTypes = GrantTypes.Code,
  //                  RequirePkce = true,
  //                  RequireClientSecret = false,

  //                  RedirectUris = { "xamarinformsclients://callback" },

  //                  AllowedScopes = {
  //                      IdentityServerConstants.StandardScopes.OpenId,
  //                      "ApiOne",
  //                  },

  //                  AllowAccessTokensViaBrowser = true,
  //                  RequireConsent = false,
  //              },
		//new Client {
  //                  ClientId = "flutter",

  //                  AllowedGrantTypes = GrantTypes.Code,
  //                  RequirePkce = true,
  //                  RequireClientSecret = false,

  //                  RedirectUris = { "http://localhost:4000/" },
  //                  AllowedCorsOrigins = { "http://localhost:4000" },

  //                  AllowedScopes = {
  //                      IdentityServerConstants.StandardScopes.OpenId,
  //                      "ApiOne",
  //                  },

  //                  AllowAccessTokensViaBrowser = true,
  //                  RequireConsent = false,
  //              },

            };
    }
}

using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {

        public IHttpClientFactory HttpClientFactory { get; }
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        [Route("/home")]
        public async Task<IActionResult> Index() 
        {

            // rectrive access token 
            var serverClient = HttpClientFactory.CreateClient();

            var descoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44377/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest 
                {
                     Address = descoveryDocument.TokenEndpoint,
                     ClientId = "client_id",
                     ClientSecret = "client_secret",
                     Scope = "ApiOne"
                });

            // rectrive secrect data
            var apiClient = HttpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44308/Secret");
            var content = await response.Content.ReadAsStringAsync();


            return Ok(new { access_token = tokenResponse.AccessToken, message = content });
        }

    }
}

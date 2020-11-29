using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth_Client.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        public HttpClient _client { get; }

        public IActionResult Index() 
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var serverResponse = await _client.GetAsync("https://localhost:44393/secret/index");

            return View();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Basic.Controllers
{
   
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret() 
        {
            return View();
        }

        public IActionResult Autenticate()
        {
            // Creating claims for a user
            var testClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.DateOfBirth,"19981105"),
                new Claim(ClaimTypes.Email,"bob@bobMail.com"),
                new Claim("Test User Says","You Can Go!"),
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, "bob@bobMail.com"),
                new Claim(ClaimTypes.Name, "Bob Salmose"),
                new Claim("DivingLicense","A+"),
            };

            // Add identity for a Claim
            var testClaimIdentity = new ClaimsIdentity(testClaims, "Test Identity");
            var licenseClaimIdentity = new ClaimsIdentity(licenseClaims, "Governtment");
            //Add identity principle
            var userPrinciple = new ClaimsPrincipal(new[] { testClaimIdentity , licenseClaimIdentity });

            // ----------------------------------------------------
            // Add user principle to the contex 
            HttpContext.SignInAsync(userPrinciple);

            return RedirectToAction("Index");
        }
    }
}

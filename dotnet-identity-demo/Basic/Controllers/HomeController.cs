﻿using Basic.CustomePolicyProvider;
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
        private readonly IAuthorizationService _authorizationService;

        // Authorization by using IAuthoriationService
        public HomeController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService; 
        }
        [AllowAnonymous]
        public IActionResult Index() 
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret() 
        {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [SecurityLevelAttribute(5)]
        public IActionResult SecretLevel()
        {
            return View("Secret");
        }

        [SecurityLevelAttribute(10)]
        public IActionResult SecretHigherLevel()
        {
            return View("Secret");
        }



        /// <summary>
        /// Role is just a Claim and it legercy 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }


        [Authorize(Policy = "AdminPolicy")]
        public IActionResult SecretRolePolicy()
        {
            return View("Secret");
        }


        /// <summary>
        /// Called by the startup
        /// </summary>
        /// <returns></returns>
        public IActionResult Autenticate()
        {
            // Creating claims for a user
            var testClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Email,"bob@bobMail.com"),
                new Claim(ClaimTypes.DateOfBirth,"199805"),
                new Claim("Friend","Good"),
                new Claim(DynamicPolicies.SecurityLevel,"7"),
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
            AuthenticationProperties authenticationProperties = new()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(1)
            };
            HttpContext.SignInAsync(userPrinciple,authenticationProperties);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DoStuff() 
        {
            // Check Authentication in inside the method
            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await _authorizationService.AuthorizeAsync(User, customPolicy);

            if (authResult.Succeeded)
            {
                return View("Index");
            }
            return View("Index");
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService
            )
        {
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index() 
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret() 
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Login Functionality
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                // sign in 
                var signInResult = await _signInManager.PasswordSignInAsync(user, password,false,false);

                if (signInResult.Succeeded)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Register");
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            // Register Functionality
            IdentityUser user = new() 
            {
                UserName = username,
                Email = ""
            };

            user.Email = "test@test.com";
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {

                // generation of the email token
               
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // generate email link

                var link = Url.Action(nameof(VerifyEmail),"Home",new { userId = user.Id , code},Request.Scheme,Request.Host.ToString());

                await _emailService.SendAsync("test@test.com","Email Verification",
                    $"<a href=\"{link}\"> Click To Verify </a>",true);


                
                //_userManager.ConfirmEmailAsync()


                //// sign user here
                //var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                //if (signInResult.Succeeded)
                //{

                //    return RedirectToAction("Index");
                //}
            }

            return RedirectToAction("EmailVerification");
        }


        public async Task<IActionResult> VerifyEmail(string userId, string code) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            

            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user,code);


            if (result.Succeeded)
            {
                return View("VerifyEmail");
            }

            return View("Register");
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}

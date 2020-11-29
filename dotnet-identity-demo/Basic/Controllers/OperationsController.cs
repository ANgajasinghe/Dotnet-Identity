using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Basic.Controllers
{ 
    public class OperationsController : Controller
    {
        public IAuthorizationService _authorizationService { get; }
        public OperationsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }



        /// <summary>
        /// This Kind of Authentication process implement only for Operations
        ///     eg:- If someone can Open, someone can TakeCookie from mu CookieJar :)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Open()
        {
            var cookieJar = new CookieJar();// Get cookie jar from database

            // check with resource;
            var result = await _authorizationService.AuthorizeAsync(User, cookieJar, CookieJarAuthOperations.Open);

            if (result.Succeeded)
            {
                return View();
            }

            return Redirect("/Home");
       


        }

        }
    }



    /// <summary>
    /// OperationAuthorizationRequirement - authorize operations (securing functions)
    /// Centralized location to manage our all requirement
    /// We control who is going through here
    /// </summary>
    public class CookieJarAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement,CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,CookieJar cookieJar)
        {
          

            // If you trying look my cookie jar
            if (requirement.Name == CookieJarOperations.Look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if(requirement.Name == CookieJarOperations.Open) 
            {
                if (context.User.HasClaim("Friend","Good"))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Stuff can do with the cookie
    /// </summary>
    public static class CookieJarOperations
    {
        public static string Open = "Open";
        public static string TakeCookie = "TakeCookie";
        public static string ComeNear = "ComeNear";
        public static string Look = "Look";

    }

    public class CookieJar
    {
        public string Name { get; set; }
        public string Size { get; set; }
    }


    public static class CookieJarAuthOperations
    {
        public static OperationAuthorizationRequirement Open = new OperationAuthorizationRequirement
        {
            Name = CookieJarOperations.Open
        };
    }



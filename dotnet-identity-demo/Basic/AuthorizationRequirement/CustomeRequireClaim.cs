﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.AuthorizationRequirement
{
    // Do you have this claim type?
    // This is the request for the above question?
    public class CustomeRequireClaim : IAuthorizationRequirement
    {
        public CustomeRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }


    // This is the handel the request
    // Centralized location to manage our all requirement
    public class CustomeRequireClaimHandler : AuthorizationHandler<CustomeRequireClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CustomeRequireClaim requirement)
        {
            var hasclaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            if (hasclaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtensions 
    {
        /// <summary>
        /// Called to this method bt Startup.cs under => AuthorizationPolicyBuilder through RequireCustomeClaim
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static AuthorizationPolicyBuilder RequireCustomeClaim(
            this AuthorizationPolicyBuilder builder, string claimType
            )
        {
            builder.AddRequirements(new CustomeRequireClaim(claimType));
            return builder; 
        }
    }

   
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.CustomePolicyProvider
{

    public class SecurityLevelAttribute : AuthorizeAttribute 
    {
        public SecurityLevelAttribute(int level)
        {
            Policy = $"{DynamicPolicies.SecurityLevel}.{level}";
        }
    }

    // {type}
    public static class DynamicPolicies 
    {
        public static IEnumerable<string> Get() 
        {
            yield return SecurityLevel;
            yield return Rank;
        }
        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
    }


    public static class DynamicAuthorizationPolicyFactory 
    {
        public static AuthorizationPolicy Create(string policyName) 
        {
            var parts = policyName.Split('.');
            var type = parts.First();
            var value = parts.Last();

            switch (type)
            {

                case DynamicPolicies.Rank:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim(DynamicPolicies.Rank, value)
                        .Build();
                case DynamicPolicies.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new SecurityLevelRequirement(Convert.ToInt32(value)))
                        .Build();
                default:
                    return null;
            }
        }
    }

    public class SecurityLevelRequirement : IAuthorizationRequirement 
    {
        public int Level { get; }
        public SecurityLevelRequirement(int level)
        {
            Level = level;
        }
    }

    public class SecurityLevelRequirementHandler : AuthorizationHandler<SecurityLevelRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            SecurityLevelRequirement requirement)
        {
            var claimValue = context.User.Claims.FirstOrDefault(x => x.Type == DynamicPolicies.SecurityLevel).Value;

            if (requirement.Level <= Convert.ToInt32(claimValue ?? "0"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }


    /// <summary>
    /// this is the custome implemetation of the  services.AddAuthorization(config =>) in Startup.cs
    /// With this *(IAuthorizationPolicyProvider) we can only override all the Policies We cannot use defult ones So,
    /// we need to use DefaultAuthorizationPolicyProvider
    /// </summary>
    public class CustomeAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomeAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {

        }

        // DO NOT CHANGE THESE UNDER ** IAuthorizationPolicyProvider **
        //public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        //{
        //    throw new NotImplementedException();
        //}


        // {type}.{value}
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach (var item in DynamicPolicies.Get())
            {
                if (policyName.StartsWith(item))
                {
                    var policy = DynamicAuthorizationPolicyFactory.Create(policyName);
                    return Task.FromResult(policy);
                }
            }

            return base.GetPolicyAsync(policyName);
        }
    }
}

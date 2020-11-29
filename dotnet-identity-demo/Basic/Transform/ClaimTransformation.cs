using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Transform
{
    public class ClaimTransformation : IClaimsTransformation
    {
        /// <summary>
        /// This Function will call evety authentication time
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasFriendClaim = principal.Claims.Any(x => x.Type == "Friend");

            if (!hasFriendClaim)
            {
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("Friend", "Bob"));
            }

            return Task.FromResult(principal); 
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class SecretController:Controller
    {
        [Route("/secret")]
        [Authorize]
        public string Index() 
        {
            // This Token not only for limited APIs But for all
            var claims = User.Claims.ToList();
            return "Sectet message from ApiOne";
        }
    }
}

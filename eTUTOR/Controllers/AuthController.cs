using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace eTUTOR.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult TokenStream()
        {
            String api_key = "apiKey123123";
            String api_secret = "MYCUSTOMCODELONGMOD4NEEDBEZE";

            var signingKey = Convert.FromBase64String(api_secret);

            JwtHeader jwtHeader = new JwtHeader(
               new SigningCredentials(
                   new SymmetricSecurityKey(signingKey),
                   SecurityAlgorithms.HmacSha512,
                   SecurityAlgorithms.RsaSha384
               )
            );

            JwtPayload jwtPayload = new JwtPayload {
                {"iss", api_key},
                {"aud", api_key},
                { "exp", ((DateTimeOffset)DateTime.UtcNow).AddHours(3).ToUnixTimeSeconds() }
            };

            var jwt = new JwtSecurityToken(jwtHeader, jwtPayload);
            var jwtHandler = new JwtSecurityTokenHandler();
            Console.Write(jwtHandler.WriteToken(jwt));

            return Json(new { TokenContext= jwtHandler.WriteToken(jwt) }, JsonRequestBehavior.AllowGet);
        }
    }
}
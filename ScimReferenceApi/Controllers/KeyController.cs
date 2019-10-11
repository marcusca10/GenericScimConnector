using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    /// <summary>
    /// Class for handeling Key endpoint. 
    /// </summary>
    [Route("api/Key")]
    public class KeyController : ControllerBase
    {
        /// <summary>
        /// Response to HTTP POST. Generates a key and returns
        /// TODO: remove or make secure.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult POST()
        {
            var tokenString = GenerateJSONWebToken();
            return Ok(new { token = tokenString });
        }

        private static string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Microsoft.Security.Bearer"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("Microsoft.Security.Bearer",
              "Microsoft.Security.Bearer",
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

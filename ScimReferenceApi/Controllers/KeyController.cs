//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    //Controller for generating a bearer token for authorization during testing
    //This is not meant to replace proper Oauth for authentication purposes. 
    //Instead this is meant to validate the bearer token authorization set up
    [Route("api/Key")]
    public class KeyController : ControllerBase
    {
        //make more secure.
        [HttpPost]
        public ActionResult POST([FromBody]string login)
        {
            if (login == "SecureLogin")
            {
                string tokenString = GenerateJSONWebToken();
                return Ok(new { token = tokenString });
            }
            return BadRequest();
        }

        private static string GenerateJSONWebToken()
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Microsoft.Security.Bearer"));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken("Microsoft.Security.Bearer",
              "Microsoft.Security.Bearer",
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}

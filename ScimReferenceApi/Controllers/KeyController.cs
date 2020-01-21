//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
    using Microsoft.IdentityModel.Tokens;

    //Controller for generating a bearer token for authorization during testing
    //This is not meant to replace proper Oauth for authentication purposes.
    //Instead this is meant to validate the bearer token authorization set up
    [Route(ControllerConstants.DefaultRouteKey)]
    public class KeyController : ControllerBase
    {
        internal const string TokenAudience = "Microsoft.Security.Bearer";
        internal const string TokenIssuer = "Microsoft.Security.Bearer";
        private const int TokenLifetimeInMins = 120;

        private static string GenerateJSONWebToken()
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KeyController.TokenIssuer));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime startTime = DateTime.UtcNow;
            DateTime expiryTime = startTime.AddMinutes(KeyController.TokenLifetimeInMins);

            JwtSecurityToken token =
                new JwtSecurityToken(
                    KeyController.TokenIssuer,
                    KeyController.TokenAudience,
                    null,
                    notBefore: startTime,
                    expires: expiryTime,
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //make more secure.
        [HttpPost]
        public ActionResult POST([FromBody]string login)
        {
            if (login == "SecureLogin")
            {
                string tokenString = GenerateJSONWebToken();
                return this.Ok(new { token = tokenString });
            }

            return this.BadRequest();
        }

    }
}

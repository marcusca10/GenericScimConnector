//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.Extensions.Logging;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    [Route("api/Me")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly ScimContext context;
        private readonly ILogger<UsersController> logger;

        public MeController(ScimContext context, ILogger<UsersController> log)
        {
            this.context = context;
            this.logger = log;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPut(ControllerConstants.Identifier)]
        public IActionResult Put()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpDelete(ControllerConstants.Identifier)]
        public IActionResult Delete()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPatch(ControllerConstants.Identifier)]
        public IActionResult Patch()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }
    }
}

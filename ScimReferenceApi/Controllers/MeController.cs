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
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;

        public MeController(ScimContext context, ILogger<UsersController> log)
        {
            this._context = context;
            this._log = log;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPut(ControllerConstants.UriID)]
        public IActionResult Put()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpDelete(ControllerConstants.UriID)]
        public IActionResult Delete()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPatch(ControllerConstants.UriID)]
        public IActionResult Patch()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }
    }
}

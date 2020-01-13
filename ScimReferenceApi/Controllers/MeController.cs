//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Services;
using Microsoft.Extensions.Logging;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    [Route("api/Me")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IProviderService<Core2User> _provider;
        //private readonly ScimContext _context;
        private readonly ILogger<UsersController> _logger;

        public MeController(IProviderService<Core2User> provider, ILogger<UsersController> log)
        {
            this._provider = provider;
            //this._context = context;
            this._logger = log;
        }

        [HttpDelete(ControllerConstants.AttributeValueIdentifier)]
        public IActionResult Delete()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPatch(ControllerConstants.AttributeValueIdentifier)]
        public IActionResult Patch()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        [HttpPut(ControllerConstants.AttributeValueIdentifier)]
        public IActionResult Put()
        {
            return this.StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }        
    }
}

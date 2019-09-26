using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Me")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly ScimContext _context;
        private readonly ILogger<UsersController> _log;
        /// <summary>
        /// 
        /// </summary>
        public MeController(ScimContext context, ILogger<UsersController> log)
        {
            _context = context;
            _log = log;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }
        /// <summary>
        /// 
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Put()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPatch("{id}")]
        public IActionResult Patch()
        {
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status501NotImplemented);
        }
    }
}

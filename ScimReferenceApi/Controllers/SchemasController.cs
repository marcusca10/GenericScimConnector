using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SchemasController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public SchemasController()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<TypeScheme> Get()
        {
            //string correlationIdentifier = null;


            //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status405MethodNotAllowed);
            IEnumerable<TypeScheme> result = new List<TypeScheme>();
            return result;
            
        }
    }
}

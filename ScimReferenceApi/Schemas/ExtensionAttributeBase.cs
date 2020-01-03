using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    public abstract class ExtensionAttributeBase
    {
        public abstract string ExtensionSchemaName
        {
            get;            
        }        
    }
}

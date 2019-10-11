using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Patch
{
    /// <summary>
    /// Abstract class for PatchRequestBase
    /// </summary>
    [DataContract]
    public abstract class PatchRequestBase : Schematized
    {
    }
}
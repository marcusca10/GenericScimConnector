//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public class EnterpriseUser : User
    {
        protected EnterpriseUser()
            : base()
        {
            this.AddSchema(SchemaIdentifiers.Core2EnterpriseUser);
        }

        [DataMember(Name = AttributeNames.ExtensionEnterpriseUser2, IsRequired = false, EmitDefaultValue = false)]
        public EnterpriseAttributes ExtentsionValues
        {
            get;
            set;
        }
    }
}

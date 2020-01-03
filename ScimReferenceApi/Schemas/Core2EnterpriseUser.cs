//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public class Core2EnterpriseUser : Core2User
    {
        protected Core2EnterpriseUser()
            : base()
        {
            this.AddSchema(SchemaIdentifiers.Core2EnterpriseUser);
        }

        [DataMember(Name = AttributeNames.ExtensionEnterpriseUser2, IsRequired = false, EmitDefaultValue = false)]
        public Core2EnterpriseUserExtension ExtentsionValues
        {
            get;
            set;
        }
    }
}

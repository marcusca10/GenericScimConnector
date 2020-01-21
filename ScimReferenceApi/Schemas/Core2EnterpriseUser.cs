//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System.Runtime.Serialization;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;

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

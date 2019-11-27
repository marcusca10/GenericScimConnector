//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    [DataContract]
    public sealed class OperationValue
    {
        private const string Template = "{0} {1}";

        [DataMember(Name = ProtocolAttributeNames.Reference, Order = 0, IsRequired = false, EmitDefaultValue = false)]
        public string Reference
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Value, Order = 1)]
        public string Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    OperationValue.Template,
                    this.Value,
                    this.Reference)
                .Trim();
            return result;
        }

        public static OperationName getOperationName(string operationName)
        {
            switch (operationName.ToLower(CultureInfo.CurrentCulture))
            {
                case "add":
                    return OperationName.Add;
                case "remove":
                    return OperationName.Remove;
                case "replace":
                    return OperationName.Replace;
                default:
                    throw new NotImplementedException("Invalid operatoin Name" + operationName);
            }
        }
    }
}

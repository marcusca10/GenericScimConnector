//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    public interface IFilter
    {
        IFilter AdditionalFilter { get; set; }
        string AttributePath { get; }
        string ComparisonValue { get; }
        string ComparisonValueEncoded { get; }
        AttributeDataType? DataType { get; set; }
        ComparisonOperator FilterOperator { get; }

        string Serialize();
    }
}

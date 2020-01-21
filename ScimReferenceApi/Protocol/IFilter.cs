//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;

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

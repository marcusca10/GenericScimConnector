//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Services;

    public static class ProviderServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderService(this IServiceCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            collection.AddTransient<IProviderService<Core2User>, UserProviderService>();
            collection.AddTransient<IProviderService<Core2Group>, GroupProviderService>();
            return collection;
        }
    }
}

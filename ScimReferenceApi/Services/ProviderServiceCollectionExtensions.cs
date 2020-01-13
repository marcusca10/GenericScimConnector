using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProviderServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderService(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            collection.AddTransient<IProviderService<Core2User>, UserProviderService>();
            collection.AddTransient<IProviderService<Core2Group>, GroupProviderService>();
            return collection;
        }
    }
}

//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
    using Newtonsoft.Json.Linq;

    public interface IProviderService<T> where T : Resource
    {
        Task Add(Resource resource);

        Task Delete(Resource resource);

        Task<Resource> GetById(string id);

        Task<Resource> GetByName(string name);

        Task<ListResponse<Resource>> Query(string query, IEnumerable<string> requested, IEnumerable<string> excluded);

        Task Replace(Resource old, Resource newresorce);

        void Update(string id, JObject body);
    }
}

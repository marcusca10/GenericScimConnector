﻿//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Services
{
    public interface IProviderService<T>
    {
        Task<ListResponse<Resource>> Query(string query, IEnumerable<string> requested, IEnumerable<string> excluded);

        Task<Resource> GetById(string id);
        Task<Resource> GetByName(string name);
        Task Add(Resource resource);
        Task Replace(Resource old, Resource newresorce);
        Task Delete(Resource resource);
        void Update(string id, JObject body);

    }
}

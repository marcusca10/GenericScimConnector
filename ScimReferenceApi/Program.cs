//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api
{    
    public static class Program
    {
        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        public static void Main(string[] args)
        {
            Program.CreateWebHostBuilder(args).Build().Run();
        }        
    }
}

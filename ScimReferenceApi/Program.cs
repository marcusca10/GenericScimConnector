//------------------------------------------------------------
// Copyright (c) 2020 Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api
{
    /// <summary>
    /// Main program.
    /// 
    /// A reference implementation for https://tools.ietf.org/html/rfc7644 and https://tools.ietf.org/html/rfc7643
    /// also look at: http://www.simplecloud.info/
    /// Logging requires "Serilog.Extensions.Logging.File" Version="1.0.1"
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>(); 
    }
}

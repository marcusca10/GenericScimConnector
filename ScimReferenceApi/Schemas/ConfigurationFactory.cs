//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.  
//------------------------------------------------------------

namespace ScimReferenceApi.Schemas
{
    using System;
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    /// <typeparam name="TException"></typeparam>
    public abstract class ConfigurationFactory<TConfiguration, TException>
        where TException: Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultConfiguration"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public abstract TConfiguration Create(
            Lazy<TConfiguration> defaultConfiguration,
            out TException error);
    }
}
//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.  
//------------------------------------------------------------

namespace ScimReferenceApi.Schemas
{
    using System;
    using System.Configuration;
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    public class ConfigurationSectionFactory<TConfiguration> : 
        ConfigurationFactory<TConfiguration, ConfigurationErrorsException> 
        where TConfiguration : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        public ConfigurationSectionFactory(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            this.SectionName = sectionName;
        }

        private string SectionName
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultConfiguration"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public override TConfiguration Create(
            Lazy<TConfiguration> defaultConfiguration,
            out ConfigurationErrorsException errors)
        {
            errors = null;

            if (null == defaultConfiguration)
            {
                throw new ArgumentNullException(nameof(defaultConfiguration));
            }

            TConfiguration result = null;
            try
            {
                result = (TConfiguration)ConfigurationManager.GetSection(this.SectionName);
            }
            catch (ConfigurationErrorsException exception)
            {
                errors = exception;
            }

            if (null == result)
            {
                result = defaultConfiguration.Value;
            }

            return result;
        }
    }    
}

﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace ScimReferenceApi.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    /// <summary>
    /// 
    /// </summary>
    public abstract class JsonFactory
    {
        private static readonly Lazy<JsonFactory> LargeObjectFactory =
            new Lazy<JsonFactory>(
                () =>
                    new TrustedJsonFactory());

        private static readonly Lazy<JsonFactory> Singleton =
            new Lazy<JsonFactory>(
                () =>
                    JsonFactory.InitializeFactory());
        /// <summary>
        /// 
        /// </summary>
        public static JsonFactory Instance
        {
            get
            {
                return JsonFactory.Singleton.Value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> Create(string json);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="acceptLargeObjects"></param>
        /// <returns></returns>
        public virtual Dictionary<string, object> Create(string json, bool acceptLargeObjects)
        {
            Dictionary<string, object> result =
                acceptLargeObjects ?
                    JsonFactory.LargeObjectFactory.Value.Create(json) :
                    this.Create(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public abstract string Create(string[] json);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="acceptLargeObjects"></param>
        /// <returns></returns>
        public virtual string Create(string[] json, bool acceptLargeObjects)
        {
            string result =
                acceptLargeObjects ?
                    JsonFactory.LargeObjectFactory.Value.Create(json) :
                    this.Create(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public abstract string Create(Dictionary<string, object> json);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="acceptLargeObjects"></param>
        /// <returns></returns>
        public virtual string Create(Dictionary<string, object> json, bool acceptLargeObjects)
        {
            string result =
                acceptLargeObjects ?
                    JsonFactory.LargeObjectFactory.Value.Create(json) :
                    this.Create(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public abstract string Create(IDictionary<string, object> json);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="acceptLargeObjects"></param>
        /// <returns></returns>
        public virtual string Create(IDictionary<string, object> json, bool acceptLargeObjects)
        {
            string result =
                acceptLargeObjects ?
                    JsonFactory.LargeObjectFactory.Value.Create(json) :
                    this.Create(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public abstract string Create(IReadOnlyDictionary<string, object> json);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="acceptLargeObjects"></param>
        /// <returns></returns>
        public virtual string Create(IReadOnlyDictionary<string, object> json, bool acceptLargeObjects)
        {
            string result =
                acceptLargeObjects ?
                    JsonFactory.LargeObjectFactory.Value.Create(json) :
                    this.Create(json);
            return result;
        }

        private static JsonFactory InitializeFactory()
        {
            JsonFactory result;
            if (SystemForCrossDomainIdentityManagementConfigurationSection.Instance.AcceptLargeObjects)
            {
                result = new TrustedJsonFactory();
            }
            else
            {
                result = new Implementation();
            }
            return result;
        }

        private class Implementation: JsonFactory
        {
            private static readonly Lazy<JavaScriptSerializer> Serializer =
                new Lazy<JavaScriptSerializer>(
                    () =>
                        new JavaScriptSerializer());

            public override Dictionary<string, object> Create(string json)
            {
                Dictionary<string, object> result = 
                    (Dictionary<string, object>)Implementation.Serializer.Value.DeserializeObject(
                        json);
                return result;
            }

            public override string Create(string[] json)
            {
                string result = Implementation.Serializer.Value.Serialize(json);
                return result;
            }

            public override string Create(Dictionary<string, object> json)
            {
                string result = Implementation.Serializer.Value.Serialize(json);
                return result;
            }

            public override string Create(IDictionary<string, object> json)
            {
                string result = Implementation.Serializer.Value.Serialize(json);
                return result;
            }

            public override string Create(IReadOnlyDictionary<string, object> json)
            {
                string result = Implementation.Serializer.Value.Serialize(json);
                return result;
            }
        }
    }
}

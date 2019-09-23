//------------------------------------------------------------
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
    public class TrustedJsonFactory: JsonFactory
    {
        private static readonly Lazy<JavaScriptSerializer> Serializer =
                new Lazy<JavaScriptSerializer>(
                    () =>
                        new JavaScriptSerializer()
                        {
                            MaxJsonLength = int.MaxValue
                        });
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override Dictionary<string, object> Create(string json)
        {
            Dictionary<string, object> result = 
                (Dictionary<string, object>)TrustedJsonFactory.Serializer.Value.DeserializeObject(
                    json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string Create(string[] json)
        {
            string result = TrustedJsonFactory.Serializer.Value.Serialize(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string Create(Dictionary<string, object> json)
        {
            string result = TrustedJsonFactory.Serializer.Value.Serialize(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string Create(IDictionary<string, object> json)
        {
            string result = TrustedJsonFactory.Serializer.Value.Serialize(json);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public override string Create(IReadOnlyDictionary<string, object> json)
        {
            string result = TrustedJsonFactory.Serializer.Value.Serialize(json);
            return result;
        }
    }
}

using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Class for scheme type at schema endpoint.
    /// </summary>
    [DataContract]
    public sealed class TypeScheme
    {

        [DataMember(Name = AttributeNames.Attributes, Order = 0)]
        private List<AttributeScheme> attributes;
        private IReadOnlyCollection<AttributeScheme> attributesWrapper;


        private object thisLock;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TypeScheme()
        {
            this.OnInitialization();
            this.OnInitialized();
        }

        /// <summary>
        /// Get the attributes.
        /// </summary>
        public IReadOnlyCollection<AttributeScheme> Attributes
        {
            get
            {
                return this.attributesWrapper;
            }
        }

        /// <summary>
        /// Get or set the scheme description.
        /// </summary>
        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Identifier.
        /// </summary>
        [DataMember(Name = AttributeNames.Identifier)]
        public string Identifier
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Name.
        /// </summary>
        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Method for adding an attribute.
        /// </summary>
        public void AddAttribute(AttributeScheme attribute)
        {
            if (null == attribute)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            Func<bool> containsFunction =
                new Func<bool>(
                        () =>
                            this
                            .attributes
                            .Any(
                                (AttributeScheme item) =>
                                    string.Equals(item.Name, attribute.Name, StringComparison.OrdinalIgnoreCase)));


            if (!containsFunction())
            {
                lock (this.thisLock)
                {
                    if (!containsFunction())
                    {
                        this.attributes.Add(attribute);
                    }
                }
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            this.OnInitialized();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            this.OnInitialization();
        }

        private void OnInitialization()
        {
            this.thisLock = new object();
            this.attributes = new List<AttributeScheme>();
        }

        private void OnInitialized()
        {
            this.attributesWrapper = this.attributes.AsReadOnly();
        }

    }
}

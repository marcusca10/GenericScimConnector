using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.SchemeEnpoint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.Attributes
{

    /// <summary>
    /// Class for attributes of the schema endpoint.
    /// </summary>
    [DataContract]
    public sealed class AttributeScheme
    {
        private AttributeDataType dataType;
        private string dataTypeValue;
        private Mutability mutability;
        private string mutabilityValue;
        private Returned returned;
        private string returnedValue;
        private Uniqueness uniqueness;
        private string uniquenessValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeScheme()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AttributeScheme(string name, AttributeDataType type, bool plural)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
            this.DataType = type;
            this.Plural = plural;
            this.Mutability = Mutability.readWrite;
            this.Returned = Returned.@default;
            this.Uniqueness = Uniqueness.none;
        }

        /// <summary>
        /// Get or set if the attribute is case exact.
        /// </summary>
        [DataMember(Name = AttributeNames.CaseExact)]
        public bool CaseExact
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set datatype.
        /// </summary>
        public AttributeDataType DataType
        {
            get
            {
                return this.dataType;
            }

            set
            {
                this.dataTypeValue = Enum.GetName(typeof(AttributeDataType), value);
                this.dataType = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Type)]
        private string DataTypeValue
        {
            get
            {
                return this.dataTypeValue;
            }

            set
            {
                var stringValue = value.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + value.Substring(1).ToLower(CultureInfo.CurrentCulture);
                this.dataType = (AttributeDataType)Enum.Parse(typeof(AttributeDataType), stringValue);
                this.dataTypeValue = value;
            }
        }

        /// <summary>
        /// Get or set the description of the attribute.
        /// </summary>
        [DataMember(Name = AttributeNames.Description)]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the mutability of the attribute.
        /// </summary>
        public Mutability Mutability
        {
            get
            {
                return this.mutability;
            }

            set
            {
                this.mutabilityValue = Enum.GetName(typeof(Mutability), value);
                this.mutability = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Mutability)]
        private string MutabilityValue
        {
            get
            {
                return this.mutabilityValue;
            }

            set
            {
                this.mutability = (Mutability)Enum.Parse(typeof(Mutability), value);
                this.mutabilityValue = value;
            }
        }

        /// <summary>
        /// Get or set the attribute name.
        /// </summary>
        [DataMember(Name = AttributeNames.Name)]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Plural.
        /// </summary>
        [DataMember(Name = AttributeNames.Plural)]
        public bool Plural
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set if the attribute is required.
        /// </summary>
        [DataMember(Name = AttributeNames.Required)]
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set if the attribute is returned.
        /// </summary>
        public Returned Returned
        {
            get
            {
                return this.returned;
            }

            set
            {
                this.returnedValue = Enum.GetName(typeof(Returned), value);
                this.returned = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Returned)]
        private string ReturnedValue
        {
            get
            {
                return this.returnedValue;
            }

            set
            {
                this.returned = (Returned)Enum.Parse(typeof(Returned), value);
                this.returnedValue = value;
            }
        }

        /// <summary>
        /// Get or set if the attribute must be unique.
        /// </summary>
        public Uniqueness Uniqueness
        {
            get
            {
                return this.uniqueness;
            }

            set
            {
                this.uniquenessValue = Enum.GetName(typeof(Uniqueness), value);
                this.uniqueness = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Called upon serialization")]
        [DataMember(Name = AttributeNames.Uniqueness)]
        private string UniquenessValue
        {
            get
            {
                return this.uniquenessValue;
            }

            set
            {
                this.uniqueness = (Uniqueness)Enum.Parse(typeof(Uniqueness), value);
                this.uniquenessValue = value;
            }
        }
    }
}
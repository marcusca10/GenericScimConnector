using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Class for holding the patch operation.
    /// </summary>
    [DataContract]
    public abstract class PatchOperationBase : IPatchOperationBase
    {
        private const string Template = "{0} {1}";

        private OperationName name;
        private string operationName;

        private IPath path;
        [DataMember(Name = ProtocolAttributeNames.Path, Order = 1)]
        private string pathExpression;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PatchOperationBase()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PatchOperationBase(OperationName operationName, string pathExpression)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(nameof(pathExpression));
            }

            this.Name = operationName;
            this.Path = Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol.Path.Create(pathExpression);
        }

        /// <summary>
        /// Get or set the op name.
        /// </summary>
        public OperationName Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.operationName = Enum.GetName(typeof(OperationName), value);
            }
        }

        /// <summary>
        /// It's the value of 'op' parameter within the json of request body.
        /// </summary>
        [DataMember(Name = ProtocolAttributeNames.Patch2Operation, Order = 0)]
        public string OperationName
        {
            get
            {
                return this.operationName;
            }

            set
            {
                if (!Enum.TryParse(value, true, out this.name))
                {
                    throw new NotSupportedException();
                }

                this.operationName = value;
            }
        }

        /// <summary>
        /// Get or set the path of the attribute to patched.
        /// </summary>
        public IPath Path
        {
            get
            {
                if (null == this.path && !string.IsNullOrWhiteSpace(this.pathExpression))
                {
                    this.path = Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol.Path.Create(this.pathExpression);
                }

                return this.path;
            }

            set
            {
                this.pathExpression = value != null ? value.ToString() : null;
                this.path = value;
            }
        }

        /// <summary>
        /// ToString overrride.
        /// </summary>
        public override string ToString()
        {
            string result =
                string.Format(
                    CultureInfo.InvariantCulture,
                    PatchOperationBase.Template,
                    this.operationName,
                    this.pathExpression);
            return result;
        }
    }
}

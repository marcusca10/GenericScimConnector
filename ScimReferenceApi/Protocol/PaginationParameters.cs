using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// Class for hodling info about pagination.
    /// </summary>
    public class PaginationParameters
    {
        int? count;
        int? startIndex;

        /// <summary>
        /// Number of resources per page.
        /// </summary>
        public int? Count
        {
            get
            {
                return this.count;
            }

            set
            {
                if (value.HasValue && value.Value < 0)
                {
                    this.count = 0;
                    return;
                }
                this.count = value;
            }
        }

        /// <summary>
        /// Start page.
        /// </summary>
        public int? StartIndex
        {
            get
            {
                return this.startIndex;
            }

            set
            {
                if (value.HasValue && value.Value < 1)
                {
                    this.startIndex = 1;
                    return;
                }
                this.startIndex = value;
            }
        }
    }
}

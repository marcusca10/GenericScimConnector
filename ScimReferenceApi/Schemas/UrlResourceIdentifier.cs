//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;

    public sealed class UrlResourceIdentifier
    {
        private const string SeparatorSegments = "/";

        private static readonly Lazy<string[]> SeperatorsSegments =
            new Lazy<string[]>(
                () =>
                    new string[]
                        {
                            UrlResourceIdentifier.SeparatorSegments
                        });

        public UrlResourceIdentifier(Uri identifier)
        {
            if (null == identifier)
            {
                throw new ArgumentNullException(nameof(identifier));
            }

            string path = identifier.OriginalString;

            // System.Uri.Segments is not supported for relative identifiers.  
            var segmentsIndexed =
                path.Split(UrlResourceIdentifier.SeperatorsSegments.Value, StringSplitOptions.None)
                .Select(
                    (string item, int index) =>
                        new
                        {
                            Segment = item,
                            Index = index
                        })
                .ToArray(); ;

            var segmentSystemForCrossDomainIdentityManagement =
                segmentsIndexed
                .LastOrDefault(
                    (item) =>
                        item.Segment.Equals(SchemaConstants.PathInterface, StringComparison.OrdinalIgnoreCase));
            if (null == segmentSystemForCrossDomainIdentityManagement)
            {
                if (identifier.IsAbsoluteUri)
                {
                    throw new ArgumentException("identifier should not be absolute");
                }
            }
            else
            {
                segmentsIndexed =
                    segmentsIndexed
                    .Where(
                        (item) =>
                            item.Index > segmentSystemForCrossDomainIdentityManagement.Index)
                    .ToArray();
            }

            IReadOnlyCollection<string> segmentsRelative =
                segmentsIndexed
                .Select(
                    (item) =>
                        item.Segment)
                .ToArray();

            string relativePath = string.Join(UrlResourceIdentifier.SeparatorSegments, segmentsRelative);

            if (!relativePath.StartsWith(UrlResourceIdentifier.SeparatorSegments, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = string.Concat(UrlResourceIdentifier.SeparatorSegments, relativePath);
            }

            this.RelativePath = relativePath;
        }

        public string RelativePath
        {
            get;
            private set;
        }
    }
}

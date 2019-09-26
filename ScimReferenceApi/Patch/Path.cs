﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Path : IPath
    {
        private const string ArgumentNamePathExpression = "pathExpression";

        private const string ConstructNameSubAttributes = "subAttr";
        private const string ConstructNameValuePath = "valuePath";
        private const string PatternTemplate = @"(?<{0}>.*)\[(?<{1}>.*)\]";
        private const string SchemaIdentifierSubnamespace =
            "urn:ietf:params:scim:schemas:";

        private static readonly string Pattern =
            string.Format(
                CultureInfo.InvariantCulture,
                Path.PatternTemplate,
                Path.ConstructNameValuePath,
                Path.ConstructNameSubAttributes);

        private static readonly Lazy<string[]> ObsoleteSchemaPrefixPatterns =
            new Lazy<string[]>(
                () =>
                    new string[]
                    {
                        "urn:scim:schemas:extension:enterprise:1.0.",
                        "urn:scim:schemas:extension:enterprise:2.0."
                    });

        private static readonly Lazy<Regex> RegularExpression =
            new Lazy<Regex>(
                () =>
                    new Regex(Path.Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));

        private const char SeperatorAttributes = '.';

        private Path(string pathExpression)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(Path.ArgumentNamePathExpression);
            }

            this.Expression = pathExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        public string AttributePath
        {
            get;
            private set;
        }

        private string Expression
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SchemaIdentifier
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<IFilter> SubAttributes
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IPath ValuePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathExpression"></param>
        /// <returns></returns>
        public static IPath Create(string pathExpression)
        {
            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(Path.ArgumentNamePathExpression);
            }

            if (!Path.TryParse(pathExpression, out IPath result))
            {
                string exceptionMessage =
                        CultureInfo.InvariantCulture+
                        "ExceptionInvalidPathTemplate"+
                        pathExpression;
                throw new ArgumentException(exceptionMessage);
            }

            return result;
        }

        private static bool TryExtractSchemaIdentifier(string pathExpression, out string schemaIdentifier)
        {
            schemaIdentifier = null;

            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                return false;
            }

            if (!pathExpression.StartsWith(Path.SchemaIdentifierSubnamespace, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            foreach (string item in Path.ObsoleteSchemaPrefixPatterns.Value)
            {
                if (pathExpression.StartsWith(item, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            int seperatorIndex =
                pathExpression.LastIndexOf(
                    SchemaConstants.SeparatorSchemaIdentifierAttribute,
                    StringComparison.OrdinalIgnoreCase);
            if (-1 == seperatorIndex)
            {
                return false;
            }

            schemaIdentifier = pathExpression.Substring(0, seperatorIndex);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool TryParse(string pathExpression, out IPath path)
        {
            path = null;

            if (string.IsNullOrWhiteSpace(pathExpression))
            {
                throw new ArgumentNullException(Path.ArgumentNamePathExpression);
            }

            Path buffer = new Path(pathExpression);

            string expression = pathExpression;

            if (Path.TryExtractSchemaIdentifier(pathExpression, out string schemaIdentifier))
            {
                expression = expression.Substring(schemaIdentifier.Length + 1);
                buffer.SchemaIdentifier = schemaIdentifier;
            }

            int seperatorIndex = expression.IndexOf(SeperatorAttributes);
            if (seperatorIndex >= 0)
            {
                string valuePathExpression = expression.Substring(seperatorIndex + 1);

                expression = expression.Substring(0, seperatorIndex);

                if (!Path.TryParse(valuePathExpression, out IPath valuePath))
                {
                    return false;
                }
                buffer.ValuePath = valuePath;
                buffer.SubAttributes = Array.Empty<IFilter>();
            }

            Match match = Path.RegularExpression.Value.Match(expression);
            if (!match.Success)
            {
                buffer.AttributePath = expression;
                buffer.SubAttributes = Array.Empty<IFilter>();
            }
            else
            {
                buffer.AttributePath = match.Groups[Path.ConstructNameValuePath].Value;
                string filterExpression = match.Groups[Path.ConstructNameSubAttributes].Value;
                if (!Filter.TryParse(filterExpression, out IReadOnlyCollection<IFilter> filters))
                {
                    return false;
                }
                buffer.SubAttributes = filters;
            }

            path = buffer;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return this.Expression;
        }
    }
}
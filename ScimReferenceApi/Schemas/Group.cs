using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.GroupAttributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Model for Core 2 Group.
    /// </summary>
    [DataContract]
    public class Group : Resource
    {
        /// <summary>
        /// Consructor.
        /// </summary>
        public Group()
        {
            this.AddSchema(SchemaIdentifiers.Core2Group);
            this.Metadata =
                new Metadata()
                {
                    ResourceType = Types.Group
                };
        }

        /// <summary>
        /// Get or set Metadata.
        /// </summary>
        [DataMember(Name = AttributeNames.Metadata)]
        public Metadata Metadata { get; set; }

        /// <summary>
        /// Get or set DisplayName.
        /// </summary>
        [DataMember(Name = AttributeNames.DisplayName)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Get or set Members.
        /// </summary>
        [DataMember(Name = AttributeNames.Members, IsRequired = false, EmitDefaultValue = false)]
        public virtual IList<Member> Members { get; set; }
    }

    /// <summary>
    /// Group extension class.
    /// </summary>
    public static class GroupExtensions
    {
        /// <summary>
        /// Returns fully populated Groups
        /// </summary>
        public static IQueryable<Group> CompleteGroups(this ScimContext context)
        {
            return context.Groups.Include("Metadata")
                    .Include("Members");
        }

        /// <summary>
        /// Method for appling patch to a group.
        /// </summary>
        public static void Apply(this Group group, PatchOperation operation)
        {
            if (null == operation)
            {
                return;
            }

            if (null == operation.Path || string.IsNullOrWhiteSpace(operation.Path.AttributePath))
            {
                return;
            }


            OperationValue value;
            switch (operation.Path.AttributePath)
            {
                case AttributeNames.DisplayName:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(group.DisplayName, value.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            value = null;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (null == value)
                    {
                        group.DisplayName = null;
                    }
                    else
                    {
                        group.DisplayName = value.Value;
                    }
                    break;
                case AttributeNames.Members:
                    group.PatchMembers(operation);
                    break;
                case AttributeNames.ExternalIdentifier:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(group.ExternalIdentifier, value.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            value = null;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (null == value)
                    {
                        group.ExternalIdentifier = null;
                    }
                    else
                    {
                        group.ExternalIdentifier = value.Value;
                    }
                    break;
                default:
                    break;
            }
        }




        /* private static void PatchMembers(this Group group, PatchOperation operation)
         {
             if (null == operation)
             {
                 return;
             }

             if
             (
                 !string.Equals(
                     Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Members,
                     operation.Path.AttributePath,
                     StringComparison.OrdinalIgnoreCase)
             )
             {
                 return;
             }

             if (null == operation.Path.ValuePath )
             {
                 return;
             }

             if (string.IsNullOrWhiteSpace(operation.Path.ValuePath.AttributePath))
             {
                 return;
             }

             IFilter subAttribute = operation.Path.SubAttributes.SingleOrDefault();
             if (null == subAttribute)
             {
                 return;
             }

             if
             (
                     (
                             operation.Value != null
                         && operation.Value.Count != 1
                     )
                 || (
                             null == operation.Value
                         && operation.Name != OperationName.Remove
                     )
             )
             {
                 return;
             }

             if
             (
                 !string.Equals(
                     Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Value,
                     subAttribute.AttributePath,
                     StringComparison.OrdinalIgnoreCase)
             )
             {
                 return;
             }

             Member address;
             Member addressExisting;
             if (group.Members != null)
             {
                 addressExisting =
                     address =
                         group
                         .Members
                         .SingleOrDefault(
                             (Member item) =>
                                 string.Equals(subAttribute.ComparisonValue, item.Value, StringComparison.Ordinal));
             }
             else
             {
                 addressExisting = null;
                 address =
                     new Member()
                     {
                         Value = subAttribute.ComparisonValue
                     };
             }

             string value;
             //if (string.Equals("value", subAttribute.ComparisonValue, StringComparison.Ordinal))
             //{
                 if
                 (
                     string.Equals(
                         Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Value,
                         operation.Path.ValuePath.AttributePath,
                         StringComparison.Ordinal)
                 )
                 {
                     value = operation.Value != null ? operation.Value.Single().Value : null;
                     if
                     (
                             value != null
                         && OperationName.Remove == operation.Name
                         && string.Equals(value, address.Value, StringComparison.OrdinalIgnoreCase)
                     )
                     {
                         value = null;
                     }
                     address.Value = value;
                 }

                 if
                 (
                     string.Equals(
                         Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.DisplayName,
                         operation.Path.ValuePath.AttributePath,
                         StringComparison.Ordinal)
                 )
                 {
                     value = operation.Value != null ? operation.Value.Single().Value : null;
                     if
                     (
                             value != null
                         && OperationName.Remove == operation.Name
                         && string.Equals(value, address.DisplayName, StringComparison.OrdinalIgnoreCase)
                     )
                     {
                         value = null;
                     }
                     address.DisplayName = value;
                 }


             //}

             if
             (
                     string.IsNullOrWhiteSpace(address.Value)
                 && string.IsNullOrWhiteSpace(address.DisplayName)
             )
             {
                 if (addressExisting != null)
                 {
                     group.Members =
                         group
                         .Members
                         .Where(
                             (Member item) =>
                                 !string.Equals(subAttribute.ComparisonValue, item.Value, StringComparison.Ordinal))
                         .ToArray();
                 }

                 return;
             }

             if (addressExisting != null)
             {
                 return;
             }

             IEnumerable<Member> addresses =
                 new Member[]
                     {
                         address
                     };
             if (null == group.Members)
             {
                 group.Members = addresses;
             }
             else
             {
                 group.Members = group.Members.Union(addresses).ToArray();
             }
         }*/



        private static void PatchMembers(this Group group, PatchOperation operation)
        {
            group.Members = PatchMembers(group.Members, operation);
        }


        internal static IList<Member> PatchMembers(IList<Member> members, PatchOperation operation)
        {
            if (!members.Any())
            {
                members = null;
            }

            if (null == operation)
            {
                return members;
            }
            if
            (
                !string.Equals(
                    AttributeNames.Members,
                    operation.Path.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return members;
            }


            if
            (
                    (
                            operation.Value != null
                        && operation.Value.Count != 1
                    )
                || (
                            null == operation.Value
                        && operation.Name != OperationName.Remove
                    )
            )
            {
                return members;
            }



            Member Member;
            Member MemberExisting;
            if (members != null && operation.OperationName != "Add")
            {
                MemberExisting =
                    Member =
                        members
                        .SingleOrDefault(
                            (Member item) =>
                                string.Equals(operation.Value?.Single().Value, item.Value, StringComparison.Ordinal));
            }
            else
            {
                MemberExisting = null;
                Member =
                    new Member()
                    {
                        //TypeName = "member"

                    };
            }

            string value = operation.Value?.Single().Value;
            if
            (
                    value != null
                && OperationName.Remove == operation.Name
                && string.Equals(value, Member.Value, StringComparison.OrdinalIgnoreCase)
            )
            {
                value = null;
            }
            Member.Value = value;

            IList<Member> result;

            if (string.IsNullOrWhiteSpace(Member.Value))
            {
                if (MemberExisting != null)
                {
                    result = members.Where((Member item) => !string.IsNullOrWhiteSpace(item.Value)).ToList();
                }
                else
                {
                    result = members;
                }
                return result;
            }

            if (MemberExisting != null)
            {
                return members;
            }


            result =
                new List<Member>
                    {
                        Member
                    };
            if (null == members)
            {
                return result;
            }

            result = members.Union(result).ToList();
            return result;

        }
    }
}

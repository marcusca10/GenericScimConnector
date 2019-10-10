using Microsoft.AzureAD.Provisioning.ScimReference.Api.Protocol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas
{
    /// <summary>
    /// Model of Core 2 User.
    /// </summary>
    [DataContract]
    public class User : Resource
    {
        /// <summary>
        /// Get or set user name.
        /// </summary>
        [DataMember(Name = AttributeNames.UserName)]
        public virtual string UserName { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public User()
        {
            this.AddSchema(SchemaIdentifiers.Core2User);
            this.Metadata =
                new Metadata()
                {
                    ResourceType = Types.User,
                };

            Active = true;
        }

        /// <summary>
        /// Get or set Active.
        /// </summary>
        [DataMember(Name = AttributeNames.Active)]
        public virtual bool Active { get; set; }

        /// <summary>
        /// Get or set Addresses.
        /// </summary>
        [DataMember(Name = AttributeNames.Addresses, IsRequired = false, EmitDefaultValue = false)]
        public virtual IEnumerable<Address> Addresses { get; set; }

        /// <summary>
        /// Get or set DisplayName, should be human readable.
        /// </summary>
        [DataMember(Name = AttributeNames.DisplayName, IsRequired = false, EmitDefaultValue = false)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Get or set ElectronicMailAddresses, JSON name email.
        /// </summary>
        [DataMember(Name = AttributeNames.ElectronicMailAddresses, IsRequired = false, EmitDefaultValue = false)]
        public virtual IEnumerable<ElectronicMailAddress> ElectronicMailAddresses { get; set; }

        /// <summary>
        /// Get or set Metadata.
        /// </summary>
        [DataMember(Name = AttributeNames.Metadata)]
        public virtual Metadata Metadata { get; set; }

        /// <summary>
        /// Get or set Name.
        /// </summary>
        [DataMember(Name = AttributeNames.Name, IsRequired = false, EmitDefaultValue = false)]
        public virtual Name Name { get; set; }

        /// <summary>
        /// Get or set PhoneNumbers.
        /// </summary>
        [DataMember(Name = AttributeNames.PhoneNumbers, IsRequired = false, EmitDefaultValue = false)]
        public virtual IEnumerable<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Get or set PreferredLanguage.
        /// </summary>
        [DataMember(Name = AttributeNames.PreferredLanguage, IsRequired = false, EmitDefaultValue = false)]
        public virtual string PreferredLanguage { get; set; }

        /// <summary>
        /// Get or set Roles.
        /// </summary>
        [DataMember(Name = AttributeNames.Roles, IsRequired = false, EmitDefaultValue = false)]
        public virtual IEnumerable<Role> Roles { get; set; }

        /// <summary>
        /// Get or set Title
        /// </summary>
        [DataMember(Name = AttributeNames.Title, IsRequired = false, EmitDefaultValue = false)]
        public virtual string Title { get; set; }
    }

    /// <summary>
    /// User extentions for queyable items
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Returns fully populated Users 
        /// </summary>
        public static IQueryable<User> CompleteUsers(this ScimContext context)
        {
            return context.Users.Include("Metadata")
                    .Include("Name")
                    .Include("ElectronicMailAddresses")
                    .Include("PhoneNumbers")
                    .Include("Roles")
                    .Include("Addresses");
        }


        /// <summary>
        /// 
        /// </summary>
        public static void Apply(this User user, PatchOperation operation)
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
                case AttributeNames.Active:
                    if (operation.Name != OperationName.Remove)
                    {
                        value = operation.Value.SingleOrDefault();
                        bool active = default(bool);
                        if (value != null && !string.IsNullOrWhiteSpace(value.Value) && bool.TryParse(value.Value, out active))
                        {
                            user.Active = active;
                        }
                    }
                    break;

                case AttributeNames.Addresses:
                    user.PatchAddresses(operation);
                    break;

                case AttributeNames.DisplayName:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(user.DisplayName, value.Value, StringComparison.OrdinalIgnoreCase))
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
                        user.DisplayName = null;
                    }
                    else
                    {
                        user.DisplayName = value.Value;
                    }
                    break;

                case AttributeNames.ElectronicMailAddresses:
                    user.PatchElectronicMailAddresses(operation);
                    break;

                case AttributeNames.ExternalIdentifier:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(user.ExternalIdentifier, value.Value, StringComparison.OrdinalIgnoreCase))
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
                        user.ExternalIdentifier = null;
                    }
                    else
                    {
                        user.ExternalIdentifier = value.Value;
                    }
                    break;


                case AttributeNames.Name:
                    user.PatchName(operation);
                    break;

                case AttributeNames.PhoneNumbers:
                    user.PatchPhoneNumbers(operation);
                    break;

                case AttributeNames.PreferredLanguage:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(user.PreferredLanguage, value.Value, StringComparison.OrdinalIgnoreCase))
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
                        user.PreferredLanguage = null;
                    }
                    else
                    {
                        user.PreferredLanguage = value.Value;
                    }
                    break;

                case AttributeNames.Roles:
                    user.PatchRoles(operation);
                    break;

                case AttributeNames.Title:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(user.Title, value.Value, StringComparison.OrdinalIgnoreCase))
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
                        user.Title = null;
                    }
                    else
                    {
                        user.Title = value.Value;
                    }
                    break;

                case AttributeNames.UserName:
                    value = operation.Value.SingleOrDefault();

                    if (OperationName.Remove == operation.Name)
                    {
                        if ((null == value) || string.Equals(user.UserName, value.Value, StringComparison.OrdinalIgnoreCase))
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
                        user.UserName = null;
                    }
                    else
                    {
                        user.UserName = value.Value;
                    }
                    break;
            }
        }

        private static void PatchAddresses(this User user, PatchOperation operation)
        {
            if (null == operation)
            {
                return;
            }

            if
            (
                !string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Addresses,
                    operation.Path.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            if (null == operation.Path.ValuePath)
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
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Type,
                    subAttribute.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            Address address;
            Address addressExisting;
            if (user.Addresses != null)
            {
                addressExisting =
                    address =
                        user
                        .Addresses
                        .SingleOrDefault(
                            (Address item) =>
                                string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal));
            }
            else
            {
                addressExisting = null;
                address =
                    new Address()
                    {
                        ItemType = subAttribute.ComparisonValue
                    };
            }

            string value;
            if (string.Equals(Address.Work, subAttribute.ComparisonValue, StringComparison.Ordinal))
            {
                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Country,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Country, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Country = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Locality,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Locality, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Locality = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.PostalCode,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.PostalCode, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.PostalCode = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Region,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Region, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Region = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.StreetAddress,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.StreetAddress, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.StreetAddress = value;
                }
                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Formatted,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Formatted, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Formatted = value;
                }
            }

            if (string.Equals(Address.Other, subAttribute.ComparisonValue, StringComparison.Ordinal))
            {
                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Country,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Country, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Country = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Locality,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Locality, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Locality = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.PostalCode,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.PostalCode, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.PostalCode = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Region,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Region, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Region = value;
                }

                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.StreetAddress,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.StreetAddress, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.StreetAddress = value;
                }
                if
                (
                    string.Equals(
                        Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Formatted,
                        operation.Path.ValuePath.AttributePath,
                        StringComparison.Ordinal)
                )
                {
                    value = operation.Value != null ? operation.Value.Single().Value : null;
                    if
                    (
                            value != null
                        && OperationName.Remove == operation.Name
                        && string.Equals(value, address.Formatted, StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        value = null;
                    }
                    address.Formatted = value;
                }
            }

            if
            (
                    string.IsNullOrWhiteSpace(address.Country)
                && string.IsNullOrWhiteSpace(address.Locality)
                && string.IsNullOrWhiteSpace(address.PostalCode)
                && string.IsNullOrWhiteSpace(address.Region)
                && string.IsNullOrWhiteSpace(address.StreetAddress)
                && string.IsNullOrWhiteSpace(address.Formatted)
            )
            {
                if (addressExisting != null)
                {
                    user.Addresses =
                        user
                        .Addresses
                        .Where(
                            (Address item) =>
                                !string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal))
                        .ToArray();
                }

                return;
            }

            if (addressExisting != null)
            {
                return;
            }

            IEnumerable<Address> addresses =
                new Address[]
                    {
                        address
                    };
            if (null == user.Addresses)
            {
                user.Addresses = addresses;
            }
            else
            {
                user.Addresses = user.Addresses.Union(addresses).ToArray();
            }
        }


        private static void PatchElectronicMailAddresses(this User user, PatchOperation operation)
        {
            user.ElectronicMailAddresses = PatchElectronicMailAddresses(user.ElectronicMailAddresses, operation);
        }

        internal static IEnumerable<ElectronicMailAddress> PatchElectronicMailAddresses(
            IEnumerable<ElectronicMailAddress> electronicMailAddresses,
            PatchOperation operation)
        {
            if (null == operation)
            {
                return electronicMailAddresses;
            }

            if
            (
                !string.Equals(
                    AttributeNames.ElectronicMailAddresses,
                    operation.Path.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return electronicMailAddresses;
            }

            if (null == operation.Path.ValuePath)
            {
                return electronicMailAddresses;
            }

            if (string.IsNullOrWhiteSpace(operation.Path.ValuePath.AttributePath))
            {
                return electronicMailAddresses;
            }

            IFilter subAttribute = operation.Path.SubAttributes.SingleOrDefault();
            if (null == subAttribute)
            {
                return electronicMailAddresses;
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
                return electronicMailAddresses;
            }

            if
            (
                !string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Type,
                    subAttribute.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return electronicMailAddresses;
            }

            string electronicMailAddressType = subAttribute.ComparisonValue;
            if
            (
                    !string.Equals(electronicMailAddressType, ElectronicMailAddress.Home, StringComparison.Ordinal)
                && !string.Equals(electronicMailAddressType, ElectronicMailAddress.Work, StringComparison.Ordinal)
            )
            {
                return electronicMailAddresses;
            }

            ElectronicMailAddress electronicMailAddress;
            ElectronicMailAddress electronicMailAddressExisting;
            if (electronicMailAddresses != null)
            {
                electronicMailAddressExisting =
                    electronicMailAddress =
                        electronicMailAddresses
                        .SingleOrDefault(
                            (ElectronicMailAddress item) =>
                                string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal));
            }
            else
            {
                electronicMailAddressExisting = null;
                electronicMailAddress =
                    new ElectronicMailAddress()
                    {
                        ItemType = electronicMailAddressType
                    };
            }



            string value = operation.Value?.Single().Value;
            if
            (
                    value != null
                && OperationName.Remove == operation.Name
                && string.Equals(value, electronicMailAddress.Value, StringComparison.OrdinalIgnoreCase)
            )
            {
                value = null;
            }
            electronicMailAddress.Value = value;

            IEnumerable<ElectronicMailAddress> result;
            if (string.IsNullOrWhiteSpace(electronicMailAddress.Value))
            {
                if (electronicMailAddressExisting != null)
                {
                    result =
                        electronicMailAddresses
                        .Where(
                            (ElectronicMailAddress item) =>
                                !string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal))
                        .ToArray();
                }
                else
                {
                    result = electronicMailAddresses;
                }
                return result;
            }

            if (electronicMailAddressExisting != null)
            {
                return electronicMailAddresses;
            }

            result =
                new ElectronicMailAddress[]
                    {
                        electronicMailAddress
                    };
            if (null == electronicMailAddresses)
            {
                return result;
            }

            result = electronicMailAddresses.Union(electronicMailAddresses).ToArray();
            return result;


        }

        private static void PatchName(this User user, PatchOperation operation)
        {
            if (null == operation)
            {
                return;
            }

            if (null == operation.Path)
            {
                return;
            }

            if
            (
                !string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Name,
                    operation.Path.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            if (null == operation.Path.ValuePath)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(operation.Path.ValuePath.AttributePath))
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

            Name nameExisting;
            Name name =
                nameExisting =
                user.Name;

            if (null == name)
            {
                name = new Name();
            }

            string value;
            if
            (
                string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.GivenName,
                    operation.Path.ValuePath.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                value = operation.Value != null ? operation.Value.Single().Value : null;
                if
                (
                        value != null
                    && OperationName.Remove == operation.Name
                    && string.Equals(value, name.GivenName, StringComparison.OrdinalIgnoreCase)
                )
                {
                    value = null;
                }
                name.GivenName = value;
            }

            if
            (
                string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.FamilyName,
                    operation.Path.ValuePath.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                value = operation.Value != null ? operation.Value.Single().Value : null;
                if
                (
                        value != null
                    && OperationName.Remove == operation.Name
                    && string.Equals(value, name.FamilyName, StringComparison.OrdinalIgnoreCase)
                )
                {
                    value = null;
                }
                name.FamilyName = value;
            }
            if
            (
                string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Formatted,
                    operation.Path.ValuePath.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                value = operation.Value != null ? operation.Value.Single().Value : null;
                if
                (
                        value != null
                    && OperationName.Remove == operation.Name
                    && string.Equals(value, name.Formatted, StringComparison.OrdinalIgnoreCase)
                )
                {
                    value = null;
                }
                name.Formatted = value;
            }




            if (string.IsNullOrWhiteSpace(name.FamilyName) && string.IsNullOrWhiteSpace(name.GivenName))
            {
                if (nameExisting != null)
                {
                    user.Name = null;
                }

                return;
            }

            if (nameExisting != null)
            {
                return;
            }

            user.Name = name;
        }

        private static void PatchPhoneNumbers(this User user, PatchOperation operation)
        {
            if (null == operation)
            {
                return;
            }

            if
            (
                !string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.PhoneNumbers,
                    operation.Path.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            if (null == operation.Path.ValuePath)
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
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.Type,
                    subAttribute.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return;
            }

            string phoneNumberType = subAttribute.ComparisonValue;
            if
            (
                    !string.Equals(phoneNumberType, PhoneNumber.Fax, StringComparison.Ordinal)
                && !string.Equals(phoneNumberType, PhoneNumber.Mobile, StringComparison.Ordinal)
                && !string.Equals(phoneNumberType, PhoneNumber.Work, StringComparison.Ordinal)
            )
            {
                return;
            }

            PhoneNumber phoneNumber;
            PhoneNumber phoneNumberExisting;
            if (user.PhoneNumbers != null)
            {
                phoneNumberExisting =
                    phoneNumber =
                        user
                        .PhoneNumbers
                        .SingleOrDefault(
                            (PhoneNumber item) =>
                                string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal));
            }
            else
            {
                phoneNumberExisting = null;
                phoneNumber =
                    new PhoneNumber()
                    {
                        ItemType = subAttribute.ComparisonValue
                    };
            }

            string value = operation.Value != null ? operation.Value.Single().Value : null;
            if
            (
                    value != null
                && OperationName.Remove == operation.Name
                && string.Equals(value, phoneNumber.Value, StringComparison.OrdinalIgnoreCase)
            )
            {
                value = null;
            }
            phoneNumber.Value = value;

            if (string.IsNullOrWhiteSpace(phoneNumber.Value))
            {
                if (phoneNumberExisting != null)
                {
                    user.PhoneNumbers =
                        user
                        .PhoneNumbers
                        .Where(
                            (PhoneNumber item) =>
                                !string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal))
                        .ToArray();
                }
                return;
            }

            if (phoneNumberExisting != null)
            {
                return;
            }

            IEnumerable<PhoneNumber> phoneNumbers =
                new PhoneNumber[]
                    {
                        phoneNumber
                    };
            if (null == user.PhoneNumbers)
            {
                user.PhoneNumbers = phoneNumbers;
            }
            else
            {
                user.PhoneNumbers = user.PhoneNumbers.Union(phoneNumbers).ToArray();
            }
        }

        private static void PatchRoles(this User user, PatchOperation operation)
        {
            user.Roles = PatchRoles(user.Roles, operation);
        }

        internal static IEnumerable<Role> PatchRoles(IEnumerable<Role> roles, PatchOperation operation)
        {
            if (null == operation)
            {
                return roles;
            }

            if
            (
                !string.Equals(
                    Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas.AttributeNames.PhoneNumbers,
                    operation.Path.AttributePath,
                    StringComparison.OrdinalIgnoreCase)
            )
            {
                return roles;
            }

            if (null == operation.Path.ValuePath)
            {
                return roles;
            }

            if (string.IsNullOrWhiteSpace(operation.Path.ValuePath.AttributePath))
            {
                return roles;
            }

            IFilter subAttribute = operation.Path.SubAttributes.SingleOrDefault();
            if (null == subAttribute)
            {
                return roles;
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
                return roles;
            }

            Role role;
            Role roleAddressExisting;
            if (roles != null)
            {
                roleAddressExisting =
                    role =
                        roles
                        .SingleOrDefault(
                            (Role item) =>
                                string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal));
            }
            else
            {
                roleAddressExisting = null;
                role =
                    new Role()
                    {
                        Primary = true
                    };
            }

            string value = operation.Value?.Single().Value;
            if
            (
                    value != null
                && OperationName.Remove == operation.Name
                && string.Equals(value, role.Value, StringComparison.OrdinalIgnoreCase)
            )
            {
                value = null;
            }
            role.Value = value;

            IEnumerable<Role> result;
            if (string.IsNullOrWhiteSpace(role.Value))
            {
                if (roleAddressExisting != null)
                {
                    result =
                        roles
                        .Where(
                            (Role item) =>
                                !string.Equals(subAttribute.ComparisonValue, item.ItemType, StringComparison.Ordinal))
                        .ToArray();
                }
                else
                {
                    result = roles;
                }
                return result;
            }

            if (roleAddressExisting != null)
            {
                return roles;
            }

            result =
                new Role[]
                    {
                        role
                    };

            if (null == roles)
            {
                return result;
            }

            result = roles.Union(roles).ToArray();
            return result;
        }
    }
}


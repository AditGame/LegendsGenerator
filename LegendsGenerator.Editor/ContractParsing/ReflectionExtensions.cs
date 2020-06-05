// -------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Relfection extensions.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets if the property is Nullable or not.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>True if nullable, false otherwise.</returns>
        public static bool IsNullable(this PropertyInfo property)
        {
            Type? enclosingType = property.DeclaringType;

            if (enclosingType == null)
            {
                throw new InvalidOperationException("Property must have an enclosing type.");
            }

            var nullable = property.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (nullable != null && nullable.ConstructorArguments.Count == 1)
            {
                var attributeArgument = nullable.ConstructorArguments[0];
                if (attributeArgument.ArgumentType == typeof(byte[]))
                {
                    ReadOnlyCollection<CustomAttributeTypedArgument>? args =
                        (ReadOnlyCollection<CustomAttributeTypedArgument>?)attributeArgument.Value;
                    if (args != null && args.Count > 0 && args[0].ArgumentType == typeof(byte))
                    {
                        return (byte)args[0].Value! == 2;
                    }
                }
                else if (attributeArgument.ArgumentType == typeof(byte))
                {
                    return (byte)attributeArgument.Value! == 2;
                }
            }

            var context = enclosingType.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
            if (context != null &&
                context.ConstructorArguments.Count == 1 &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte)context.ConstructorArguments[0].Value! == 2;
            }

            // Couldn't find a suitable attribute
            return false;
        }
    }
}

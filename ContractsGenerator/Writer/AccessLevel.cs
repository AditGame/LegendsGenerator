// <copyright file="AccessLevel.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.ContractsGenerator.Writer
{
    /// <summary>
    /// The access level.
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// Private access level.
        /// </summary>
        Private,

        /// <summary>
        /// Internal access level.
        /// </summary>
        Internal,

        /// <summary>
        /// Protected access level.
        /// </summary>
        Protected,

        /// <summary>
        /// Protected Internal access level.
        /// </summary>
        ProtectedInternal,

        /// <summary>
        /// Public access level.
        /// </summary>
        Public,
    }

    /// <summary>
    /// extension methods for AccessLevel.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name. In this case file name should match enum name.
    public static class AccessLevelExtensions
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Gets the C# representation of the access level.
        /// </summary>
        /// <param name="access">The access level.</param>
        /// <returns>The access level the compiler expects in C# source files.</returns>
        public static string AccessString(this AccessLevel access)
        {
            switch (access)
            {
                case AccessLevel.ProtectedInternal:
                    return "protected internal";
                default:
                    return access.ToString().ToLower();
            }
        }
    }
}

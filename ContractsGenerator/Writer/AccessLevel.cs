// <copyright file="SourceWriter.cs" company="Tom Luppi">
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
    public static class AccessLevelExtensions
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

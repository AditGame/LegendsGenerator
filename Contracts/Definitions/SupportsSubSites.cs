// <copyright file="SupportsSubSites.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Gets the number of subsites a site supports.
    /// </summary>
    public partial class SupportsSubSites : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the condition which decides if subsites are allowed at all.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string Condition { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the condition which returns the number of subsites this site supports.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string NumberOfSubSites { get; set; } = UnsetString;
    }
}

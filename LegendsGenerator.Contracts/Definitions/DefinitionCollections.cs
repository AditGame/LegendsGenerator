// <copyright file="DefinitionCollections.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A collection of all definitions.
    /// </summary>
    public class DefinitionCollections
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionCollections"/> class.
        /// </summary>
        /// <param name="sites">The list of parsed site definitions.</param>
        public DefinitionCollections(IList<SiteDefinition> sites)
        {
            this.SiteDefinitions = sites.ToList();
        }

        /// <summary>
        /// Gets the list of all site definitions.
        /// </summary>
        public IReadOnlyList<SiteDefinition> SiteDefinitions { get; }
    }
}

// <copyright file="DefinitionCollections.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts.Compiler;

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

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public IReadOnlyList<BaseThingDefinition> AllDefinitions =>
            this.SiteDefinitions;

        /// <summary>
        /// Attaches the compiler to all definitions.
        /// </summary>
        /// <param name="compiler">The csmpiler.</param>
        public void Attach(IConditionCompiler compiler)
        {
            foreach (var def in this.AllDefinitions)
            {
                def.Attach(compiler);
            }
        }
    }
}

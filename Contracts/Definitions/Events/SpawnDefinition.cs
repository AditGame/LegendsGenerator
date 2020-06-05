// <copyright file="SpawnDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System.Collections.Generic;

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// A definition of a result which spawns a new thing.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class SpawnDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the name of the definition to spawn.
        /// </summary>
        public string DefinitionName { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the type of thing to spawn.
        /// </summary>
        public ThingType Type { get; set; }

        /// <summary>
        /// Gets the attribute overrides for this spawn.
        /// </summary>
        [CompiledDictionary(typeof(int), "Subject")]
        public Dictionary<string, string> AttributeOverrides { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the description of this spawn result.
        /// </summary>
        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Description { get; set; } = UnsetString;
    }
}

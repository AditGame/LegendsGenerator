// <copyright file="SpawnDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using System.Collections.Generic;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// A definition of a result which spawns a new thing.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class SpawnDefinition : BaseEffectDefinition
    {
        /// <summary>
        /// Gets or sets the name of the definition to spawn.
        /// </summary>
        public string DefinitionNameToSpawn { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the type of thing to spawn.
        /// </summary>
        public ThingType Type { get; set; }

        /// <summary>
        /// Gets the attribute overrides for this spawn.
        /// </summary>
        [CompiledDictionary(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public Dictionary<string, string> AttributeOverrides { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the type of position.
        /// </summary>
        public PositionType PositionType { get; set; }

        /// <summary>
        /// Gets or sets the X Position.
        /// </summary>
        [Compiled(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string PositionX { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the Y position.
        /// </summary>
        [Compiled(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string PositionY { get; set; } = UnsetString;
    }
}

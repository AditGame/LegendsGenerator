// <copyright file="EventResultDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>


namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System.Collections.Generic;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events.Effects;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The result of an event occurring.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class EventResultDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this result will be picked if no other result is applicable.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Gets or sets a string which returns a number between 1 and 100 representing the chance of this happening.
        /// </summary>
        [Compiled(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string Chance { get; set; } = "100";

        /// <summary>
        /// Gets or sets the string condition of this event.
        /// </summary>
        [Compiled(typeof(bool))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string Condition { get; set; } = "true";

        /// <summary>
        /// Gets or sets the effects of this result.
        /// </summary>
        public List<AttributeEffectDefinition> Effects { get; set; } = new List<AttributeEffectDefinition>();

        /// <summary>
        /// Gets or sets the spawns of this result.
        /// </summary>
        public List<SpawnDefinition> Spawns { get; set; } = new List<SpawnDefinition>();

        /// <summary>
        /// Gets or sets the transforms of this result.
        /// </summary>
        public List<TransformDefinition> Transforms { get; set; } = new List<TransformDefinition>();

        /// <summary>
        /// Gets or sets the things destroyed in this result.
        /// </summary>
        public List<DestroyDefinition> Destroys { get; set; } = new List<DestroyDefinition>();

        /// <summary>
        /// Gets or sets whatever moves as a result.
        /// </summary>
        public List<MoveDefinition> Moves { get; set; } = new List<MoveDefinition>();
    }
}

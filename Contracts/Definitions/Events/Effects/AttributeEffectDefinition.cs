// <copyright file="EffectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// The effect definition.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class AttributeEffectDefinition : BaseEffectDefinitionWithDuration
    {
        /// <summary>
        /// Gets or sets the attribute effected by this.
        /// </summary>
        public string AffectedAttribute { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the magnitude of the change.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string Magnitude { get; set; } = UnsetString;
    }
}

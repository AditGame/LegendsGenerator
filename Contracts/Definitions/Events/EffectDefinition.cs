// <copyright file="EffectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System.Collections.Generic;

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// The effect definition.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class EffectDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the members this effect effects.
        /// </summary>
        public List<string> AppliedTo { get; set; } = new List<string>() { "Subject" };

        /// <summary>
        /// Gets or sets the attribute effected by this.
        /// </summary>
        public string AffectedAttribute { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the magnitude of the change.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string Magnitude { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the duration of the change.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string Duration { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the title of the effect.
        /// </summary>
        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Title { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the description of the effect.
        /// </summary>
        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Description { get; set; } = UnsetString;
    }
}

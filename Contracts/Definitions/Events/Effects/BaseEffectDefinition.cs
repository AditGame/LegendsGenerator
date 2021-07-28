// <copyright file="BaseEffectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using System.Collections.ObjectModel;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Base definition of an effect.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class BaseEffectDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the members this effect effects.
        /// </summary>
        public Collection<string> AppliedTo { get; set; } = new Collection<string>() { "Subject" };

        /// <summary>
        /// Gets or sets the title of the effect.
        /// </summary>
        [Compiled(typeof(string), AsFormattedText = true)]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string Title { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the description of the effect.
        /// </summary>
        [Compiled(typeof(string), AsFormattedText = true)]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string Description { get; set; } = UnsetString;
    }
}

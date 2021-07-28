// -------------------------------------------------------------------------------------------------
// <copyright file="QuestDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Definitions.Validation;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Definition of a quest.
    /// </summary>
    public partial class QuestDefinition : BaseThingDefinition
    {
        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Quest;

        /// <summary>
        /// Gets or sets the name of the thing, such as Scholar.
        /// </summary>
        [ControlsDefinitionName]
        [Compiled(typeof(string), AsFormattedText = true)]
        [CompiledVariable("Subject", typeof(BaseThing))]
        public override string Name { get; set; } = UnsetString;
    }
}

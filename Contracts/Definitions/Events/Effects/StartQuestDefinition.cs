// <copyright file="StartQuestDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using System.Collections.Generic;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Defines an effect which starts a quest.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class StartQuestDefinition : BaseEffectDefinition
    {
        /// <summary>
        /// Gets or sets the name of the quest to start.
        /// </summary>
        public string QuestNameToStart { get; set; } = UnsetString;

        /// <summary>
        /// Gets the attribute overrides for this spawn.
        /// </summary>
        [CompiledDictionary(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BasePhysicalThing))]
        public Dictionary<string, string> AttributeOverrides { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets the aspect overrides for this spawn.
        /// </summary>
        [CompiledDictionary(typeof(string))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BasePhysicalThing))]
        public Dictionary<string, string> AspectOverrides { get; } = new Dictionary<string, string>();
    }
}

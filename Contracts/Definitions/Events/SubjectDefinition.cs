// <copyright file="SubjectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System.Collections.ObjectModel;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// A subject is what an event acts on, this lets you select the subject.
    /// </summary>
    public partial class SubjectDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the condition on the thing to scope on.
        /// </summary>
        [Compiled(typeof(bool))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string Condition { get; set; } = "true";

        /// <summary>
        /// Gets or sets the type of thing this scope relates to.
        /// </summary>
        public ThingType Type { get; set; } = ThingType.None;

        /// <summary>
        /// Gets or sets the applicable Definition names which this relates to.
        /// If empty, any Definition is allowed.
        /// </summary>
        public Collection<string> Definitions { get; set; } = new Collection<string>();

        /// <summary>
        /// Gets or sets the list of quests the Subject must have to match (Must have all quests).
        /// If empty, quests are not considered when matching.
        /// </summary>
        public Collection<string> Quests { get; set; } = new Collection<string>();
    }
}

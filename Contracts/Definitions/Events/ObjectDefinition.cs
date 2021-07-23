// <copyright file="ObjectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The definition of an object in an event.
    /// </summary>
    public partial class ObjectDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets a value indicating whether this object should be optional.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Gets or sets the taxicab distance from the subject to consider objects.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or sets the condition on the thing to scope on.
        /// </summary>
        [Compiled(typeof(bool))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        [CompiledVariable("Object", typeof(BaseThing))]
        public string Condition { get; set; } = "true";

        /// <summary>
        /// Gets or sets the condition to Maximize when selecting an Object. If null, no condition is maximized and a randomly matching thing is selected.
        /// </summary>
        [Compiled(typeof(float))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        [CompiledVariable("Object", typeof(BaseThing))]
        public string? Maximize { get; set; }

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
        /// Gets or sets the list of quests the Object must have to match (Must have all quests).
        /// If empty, quests are not considered when matching.
        /// </summary>
        public Collection<string> Quests { get; set; } = new Collection<string>();

        /// <summary>
        /// Gets the type of the Object variable.
        /// </summary>
        /// <returns>The actual type of Object.</returns>
        public Type TypeOfObject()
        {
            return this.Type.AssociatedType();
        }
    }
}

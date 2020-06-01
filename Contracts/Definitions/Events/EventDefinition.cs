// <copyright file="EventDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// The definition of a single event.
    /// </summary>
    public partial class EventDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the event Condition, from one to one hundred.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string Chance { get; set; } = "0";

        /// <summary>
        /// Gets or sets the subject of this event.
        /// </summary>
        public SubjectDefinition Subject { get; set; }

        /// <summary>
        /// Gets or sets the objects of this event.
        /// </summary>
        public IDictionary<string, ObjectDefinition> Objects { get; set; } = new Dictionary<string, ObjectDefinition>();

        /// <summary>
        /// Gets or sets the descriptino of this event.
        /// </summary>
        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Description { get; set; } = "UNDEFINED_DESCRIPTION";

        /// <summary>
        /// Gets or sets the results of this event.
        /// </summary>
        public EventResultDefinition[] Results { get; set; } = Array.Empty<EventResultDefinition>();

        /// <summary>
        /// Gets additional variable names for the Description method.
        /// </summary>
        /// <returns>The list of additional parameters.</returns>
        public IList<string> AdditionalParametersForClass()
        {
            return this.Objects?.Select(x => x.Key).ToList() ?? new List<string>();
        }
    }
}

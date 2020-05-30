// <copyright file="EventDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    /// <summary>
    /// The definition of a single event.
    /// </summary>
    public class EventDefinition
    {
        /// <summary>
        /// Gets or sets the event Condition, from one to one hundred.
        /// </summary>
        public string Chance { get; set; }

        /// <summary>
        /// Gets or sets the subject of this event.
        /// </summary>
        public SubjectDefinition Subject { get; set; }

        /// <summary>
        /// Getsor sets the objects of this event.
        /// </summary>
        public ObjectDefinition[] Objects { get; set; }

        /// <summary>
        /// Gets or sets the descriptino of this event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the results of this event.
        /// </summary>
        public EventResultDefinition[] Results { get; set; }
    }
}

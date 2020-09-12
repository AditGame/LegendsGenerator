// -------------------------------------------------------------------------------------------------
// <copyright file="OccurredEvent.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator
{
    using System.Collections.Generic;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// An event which occurred.
    /// </summary>
    public record OccurredEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OccurredEvent"/> class.
        /// </summary>
        /// <param name="description">The description of the event.</param>
        /// <param name="eventDefiniton">The event definition.</param>
        /// <param name="subject">The subject of the definition.</param>
        /// <param name="objects">The objects of the evevnt.</param>
        public OccurredEvent(string description, EventDefinition eventDefiniton, BaseThing subject, IDictionary<string, BaseThing> objects)
        {
            this.Description = description;
            this.Event = eventDefiniton;
            this.Subject = subject;
            this.Objects = objects;
        }

        /// <summary>
        /// Gets the description of the event.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the event which occurred.
        /// </summary>
        public EventDefinition Event { get; init; }

        /// <summary>
        /// Gets the subject of the event.
        /// </summary>
        public BaseThing Subject { get; init; }

        /// <summary>
        /// Gets the objects which were involved in the event.
        /// </summary>
        public IDictionary<string, BaseThing> Objects { get; init; }

        /// <summary>
        /// Gets or sets the result of the event.
        /// </summary>
        public EventResultDefinition? Result { get; set; }
    }
}

// <copyright file="EventCollection.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LegendsGenerator.Contracts.Definitions.Events;

    public class EventCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCollection"/> class.
        /// </summary>
        /// <param name="events">The list of parsed events.</param>
        public EventCollection(IList<EventDefinition> events)
        {
            this.Events = events.ToList();
        }

        /// <summary>
        /// Gets all definitions.
        /// </summary>
        public IReadOnlyList<EventDefinition> Events { get; }
    }
}

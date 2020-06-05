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
    public class OccurredEvent
    {
        /// <summary>
        /// Gets or sets the event which occurred.
        /// </summary>
        public EventDefinition Event { get; set; } = new EventDefinition();

        /// <summary>
        /// Gets or sets the objects which were involved in the event.
        /// </summary>
        public IDictionary<string, BaseThing> Objects { get; set; } = new Dictionary<string, BaseThing>();
    }
}

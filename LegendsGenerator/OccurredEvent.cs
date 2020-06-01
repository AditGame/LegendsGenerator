namespace LegendsGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        public EventDefinition Event { get; set; }

        /// <summary>
        /// Gets or sets the objects which were involved in the event.
        /// </summary>
        public IDictionary<string, BaseThing> Objects { get; set; }
    }
}

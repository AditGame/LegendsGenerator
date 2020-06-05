// <copyright file="DefinitionFile.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The structure of a definition file.
    /// </summary>
    public class DefinitionFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionFile"/> class.
        /// </summary>
        public DefinitionFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionFile"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        /// <param name="events">The list of events.</param>
        public DefinitionFile(DefinitionCollections definitions, EventCollection events)
        {
            this.Sites = definitions.SiteDefinitions.ToList();
            this.Events = events.Events.ToList();
        }

        /// <summary>
        /// Gets or sets the list of all events.
        /// </summary>
        public List<EventDefinition> Events { get; set; } = new List<EventDefinition>();

        /// <summary>
        /// Gets or sets the list of site definitions.
        /// </summary>
        public List<SiteDefinition> Sites { get; set; } = new List<SiteDefinition>();
    }
}

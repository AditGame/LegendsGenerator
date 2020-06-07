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
        public DefinitionFile(DefinitionCollection definitions)
        {
            this.Events = definitions.Events.ToList();
            this.Sites = definitions.SiteDefinitions.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionFile"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        public DefinitionFile(IEnumerable<BaseDefinition> definitions)
        {
            ILookup<System.Type, BaseDefinition>? byType = definitions.ToLookup(d => d.GetType());
            this.Events = byType[typeof(EventDefinition)].OfType<EventDefinition>().ToList();
            this.Sites = byType[typeof(SiteDefinition)].OfType<SiteDefinition>().ToList();
        }

        /// <summary>
        /// Gets or sets the list of all events.
        /// </summary>
        public List<EventDefinition> Events { get; set; } = new List<EventDefinition>();

        /// <summary>
        /// Gets or sets the list of site definitions.
        /// </summary>
        public List<SiteDefinition> Sites { get; set; } = new List<SiteDefinition>();

        /// <summary>
        /// Gets all definitions in this file.
        /// </summary>
        /// <returns>All defintions.</returns>
        public IEnumerable<BaseDefinition> AllDefinitions()
        {
            List<BaseDefinition> defs = new List<BaseDefinition>();
            defs.AddRange(this.Events.OfType<BaseDefinition>());
            defs.AddRange(this.Sites.OfType<BaseDefinition>());
            return defs;
        }
    }
}

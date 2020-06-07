// <copyright file="DefinitionCollections.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// A collection of all definitions.
    /// </summary>
    public class DefinitionCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionCollection"/> class.
        /// </summary>
        /// <param name="events">The list of parsed event definitions.</param>
        /// <param name="sites">The list of parsed site definitions.</param>
        public DefinitionCollection(
            IList<EventDefinition> events,
            IList<SiteDefinition> sites)
        {
            this.Events = events.ToList();
            this.SiteDefinitions = sites.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionCollection"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        public DefinitionCollection(IEnumerable<BaseDefinition> definitions)
        {
            ILookup<System.Type, BaseDefinition>? byType = definitions.ToLookup(d => d.GetType());
            this.Events = byType[typeof(EventDefinition)].OfType<EventDefinition>().ToList();
            this.SiteDefinitions = byType[typeof(SiteDefinition)].OfType<SiteDefinition>().ToList();
        }

        /// <summary>
        /// Gets all definitions.
        /// </summary>
        public IReadOnlyList<EventDefinition> Events { get; }

        /// <summary>
        /// Gets the list of all site definitions.
        /// </summary>
        public IReadOnlyList<SiteDefinition> SiteDefinitions { get; }

        /// <summary>
        /// Gets the list of all definitions.
        /// </summary>
        public IReadOnlyList<BaseDefinition> AllDefinitions
        {
            get
            {
                List<BaseDefinition> defs = new List<BaseDefinition>();
                defs.AddRange(this.Events.OfType<BaseDefinition>());
                defs.AddRange(this.SiteDefinitions.OfType<BaseDefinition>());
                return defs;
            }
        }

        /// <summary>
        /// Attaches the compiler to all definitions.
        /// </summary>
        /// <param name="compiler">The csmpiler.</param>
        public void Attach(IConditionCompiler compiler)
        {
            foreach (var def in this.AllDefinitions)
            {
                def.Attach(compiler);
            }
        }
    }
}

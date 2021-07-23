// <copyright file="DefinitionFile.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.Json.Serialization;
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
        public DefinitionFile(DefinitionsCollection definitions)
        {
            this.AllDefinitions = definitions.AllDefinitions.ToCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionFile"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        public DefinitionFile(IEnumerable<BaseDefinition> definitions)
        {
            this.AllDefinitions = definitions.ToCollection();
        }

        /// <summary>
        /// Gets or sets all definitions in this object.
        /// </summary>
        [JsonIgnore]
        public Collection<BaseDefinition> AllDefinitions { get; set; } = new Collection<BaseDefinition>();

        /// <summary>
        /// Gets or sets the list of all events.
        /// </summary>
        public Collection<EventDefinition> Events
        {
            get
            {
                return this.AllDefinitions.OfType<EventDefinition>().ToCollection();
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(EventDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }

        /// <summary>
        /// Gets or sets the list of site definitions.
        /// </summary>
        public Collection<SiteDefinition> Sites
        {
            get
            {
                return new Collection<SiteDefinition>(this.AllDefinitions.OfType<SiteDefinition>().ToCollection());
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(SiteDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }

        /// <summary>
        /// Gets or sets the list of notable person definitions.
        /// </summary>
        public Collection<NotablePersonDefinition> NotablePeople
        {
            get
            {
                return this.AllDefinitions.OfType<NotablePersonDefinition>().ToCollection();
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(NotablePersonDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }

        /// <summary>
        /// Gets or sets the list of notable person definitions.
        /// </summary>
        public Collection<WorldSquareDefinition> WorldSquares
        {
            get
            {
                return this.AllDefinitions.OfType<WorldSquareDefinition>().ToCollection();
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(WorldSquareDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }

        /// <summary>
        /// Gets or sets the list of world definitions.
        /// </summary>
        public Collection<WorldDefinition> World
        {
            get
            {
                return this.AllDefinitions.OfType<WorldDefinition>().ToCollection();
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(WorldDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }

        /// <summary>
        /// Gets or sets the list of quest definitions.
        /// </summary>
        public Collection<QuestDefinition> Quests
        {
            get
            {
                return this.AllDefinitions.OfType<QuestDefinition>().ToCollection();
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(QuestDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }

        /// <summary>
        /// Gets or sets the list of unit definitions.
        /// </summary>
        public Collection<UnitDefinition> Units
        {
            get
            {
                return this.AllDefinitions.OfType<UnitDefinition>().ToCollection();
            }

            set
            {
                var defs = this.AllDefinitions.Where(x => x.GetType() != typeof(UnitDefinition)).ToList();
                defs.AddRange(value);
                this.AllDefinitions = defs.ToCollection();
            }
        }
    }
}

// <copyright file="DefinitionFile.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
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
        public DefinitionFile(DefinitionCollection definitions)
        {
            this.AllDefinitions = definitions.AllDefinitions.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionFile"/> class.
        /// </summary>
        /// <param name="definitions">The list of definitions.</param>
        public DefinitionFile(IEnumerable<BaseDefinition> definitions)
        {
            this.AllDefinitions = definitions.ToList();
        }

        /// <summary>
        /// Gets or sets all definitions in this object.
        /// </summary>
        [JsonIgnore]
        public List<BaseDefinition> AllDefinitions { get; set; } = new List<BaseDefinition>();

        /// <summary>
        /// Gets or sets the list of all events.
        /// </summary>
        public List<EventDefinition> Events
        {
            get
            {
                return this.AllDefinitions.OfType<EventDefinition>().ToList();
            }

            set
            {
                this.AllDefinitions = this.AllDefinitions.Where(x => x.GetType() != typeof(EventDefinition)).ToList();
                this.AllDefinitions.AddRange(value);
            }
        }
            

        /// <summary>
        /// Gets or sets the list of site definitions.
        /// </summary>
        public List<SiteDefinition> Sites
        {
            get
            {
                return this.AllDefinitions.OfType<SiteDefinition>().ToList();
            }

            set
            {
                this.AllDefinitions = this.AllDefinitions.Where(x => x.GetType() != typeof(SiteDefinition)).ToList();
                this.AllDefinitions.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or sets the list of notable person definitions.
        /// </summary>
        public List<NotablePersonDefinition> NotablePeople
        {
            get
            {
                return this.AllDefinitions.OfType<NotablePersonDefinition>().ToList();
            }

            set
            {
                this.AllDefinitions = this.AllDefinitions.Where(x => x.GetType() != typeof(NotablePersonDefinition)).ToList();
                this.AllDefinitions.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or sets the list of notable person definitions.
        /// </summary>
        public List<WorldSquareDefinition> WorldSquares
        {
            get
            {
                return this.AllDefinitions.OfType<WorldSquareDefinition>().ToList();
            }

            set
            {
                this.AllDefinitions = this.AllDefinitions.Where(x => x.GetType() != typeof(WorldSquareDefinition)).ToList();
                this.AllDefinitions.AddRange(value);
            }
        }
    }
}

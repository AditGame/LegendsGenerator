// <copyright file="BaseThingDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// THe base definition of a thing.
    /// </summary>
    public partial class BaseThingDefinition : BaseDefinition, ITopLevelDefinition
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public string SourceFile { get; set; } = UnsetString;

        /// <inheritdoc/>
        [JsonIgnore]
        string ITopLevelDefinition.DefinitionName
        {
            get => this.Name;
            set => this.Name = value;
        }

        /// <summary>
        /// Gets or sets the full list of all inherited definition names (including the top level).
        /// </summary>
        /// <remarks>This is filled during the thing creation process. It should be cleaned up in the future.</remarks>
        [JsonIgnore]
        [EditorIgnore]
        public IList<string> InheritedDefinitionNames { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the name of the thing, such as Scholar.
        /// </summary>
        [ControlsDefinitionName]
        public string Name { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the description of the thing.
        /// </summary>
        public string Description { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the string which returns the maximum number of events.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string MaxEvents { get; set; } = "1";

        /// <summary>
        /// Gets or sets the name of the site this inherits from.
        /// </summary>
        public string? InheritsFrom { get; set; }

        /// <summary>
        /// Gets or sets the default attributes on a Thing.
        /// </summary>
        [CompiledDictionary(typeof(int))]
        public Dictionary<string, string> DefaultAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the default aspects on a Thing.
        /// </summary>
        [CompiledDictionary(typeof(string))]
        public Dictionary<string, string> DefaultAspects { get; set; } = new Dictionary<string, string>();
    }
}

// <copyright file="BaseThingDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Definitions.Validation;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// THe base definition of a thing.
    /// </summary>
    public abstract partial class BaseThingDefinition : BaseDefinition, ITopLevelDefinition
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
        /// Gets the thing type this represents.
        /// </summary>
        [JsonIgnore]
        public abstract ThingType ThingType { get; }

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
        [Compiled(typeof(int))]
        [CompiledVariable("Subject", typeof(BaseThing))]
        [EditorIcon(EditorIcon.CompiledDynamic)]
        public string MaxEvents { get; set; } = "1";

        /// <summary>
        /// Gets or sets the name of the site this inherits from.
        /// </summary>
        public string? InheritsFrom { get; set; }

        /// <summary>
        /// Gets or sets the attributes on the thing.
        /// </summary>
        public Dictionary<string, AttributeDefinition> Attributes { get; set; } = new Dictionary<string, AttributeDefinition>();

        /// <summary>
        /// Gets or sets the aspects on the thing.
        /// </summary>
        public Dictionary<string, AspectDefinition> Aspects { get; set; } = new Dictionary<string, AspectDefinition>();

        /// <summary>
        /// Gets the type of Subject.
        /// </summary>
        /// <returns>The type of the subject in this thing.</returns>
        public Type TypeOfSubject()
        {
            return this.ThingType.AssociatedType();
        }
    }
}

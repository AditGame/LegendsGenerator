// <copyright file="BaseThingDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Collections.Generic;
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// THe base definition of a thing.
    /// </summary>
    public partial class BaseThingDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the name of the thing, such as Scholar.
        /// </summary>
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

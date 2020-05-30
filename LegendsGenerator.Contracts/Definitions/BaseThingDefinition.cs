// <copyright file="BaseThingDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using CompiledDefinitionSourceGenerator;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// THe base definition of a thing.
    /// </summary>
    [XmlInclude(typeof(SiteDefinition))]
    public class BaseThingDefinition
    {
        /// <summary>
        /// Gets or sets the name of the thing, such as Scholar.
        /// </summary>
        public string Name { get; set; } = "UNSET";

        /// <summary>
        /// Gets or sets the description of the thing.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the string which returns the maximum number of events.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string MaxEvents { get; set; } = "1";

        /// <summary>
        /// Gets or sets the name of the site this inherits from.
        /// </summary>
        public string? InheritsFrom { get; set; }

        /// <summary>
        /// Gets or sets the default attributes on a Thing.
        /// </summary>
        public Dictionary<string, string> DefaultAttributes { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the default aspects on a Thing.
        /// </summary>
        public Dictionary<string, string> DefaultAspects { get; set; } = new Dictionary<string, string>();
    }
}

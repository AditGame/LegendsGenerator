// <copyright file="SiteDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using CompiledDefinitionSourceGenerator;

    /// <summary>
    /// The definition for a site.
    /// </summary>
    [NeedsCompiling]
    public partiaclass SiteDefinition : BaseThingDefinition
    {
        /// <summary>
        /// Gets or sets if this site supports sites inside of itself.
        /// </summary>
        public SupportsSubSites? SubSites { get; set; }
    }
}

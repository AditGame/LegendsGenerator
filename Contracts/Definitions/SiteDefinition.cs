// <copyright file="SiteDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// The definition for a site.
    /// </summary>
    public partial class SiteDefinition : BaseThingDefinition
    {
        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Site;
    }
}

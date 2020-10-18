// <copyright file="SiteDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

using LegendsGenerator.Contracts.Definitions.Events;

namespace LegendsGenerator.Contracts.Definitions
{
    /// <summary>
    /// The definition for a site.
    /// </summary>
    public partial class SiteDefinition : BaseThingDefinition
    {
        public override ThingType ThingType => ThingType.Site;
    }
}

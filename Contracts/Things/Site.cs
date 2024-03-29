﻿// <copyright file="Site.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// An instance of a Site in the world.
    /// </summary>
    public record Site : BasePhysicalThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="rdm">Random number generator.</param>
        public Site(SiteDefinition definition, Random rdm)
            : base(definition, rdm)
        {
        }

        /// <summary>
        /// Gets the site definition.
        /// </summary>
        public SiteDefinition Definition => this.BaseDefinition as SiteDefinition ?? throw new InvalidOperationException("Definition time is wrong type.");

        /// <summary>
        /// Gets the name of this Thing Type.
        /// </summary>
        public override ThingType ThingType => ThingType.Site;
    }
}

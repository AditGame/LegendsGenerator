// <copyright file="Site.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// TAn instance of a Site in the world.
    /// </summary>
    public class Site : BaseThing
    {
        /// <summary>
        /// Gets the site definition.
        /// </summary>
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        public SiteDefinition Definition => this.BaseDefinition as SiteDefinition ?? throw new InvalidOperationException("Definition time is wrong type.");
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations

        /// <summary>
        /// Gets the name of this Thing Type.
        /// </summary>
        public override string ThingTypeName => "Site";
    }
}

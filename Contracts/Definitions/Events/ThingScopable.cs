// <copyright file="ThingScopable.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Defines a scope for a set of things.
    /// </summary>
    public partial class ThingScopable : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the type of thing this scope relates to.
        /// </summary>
        public ThingType Type { get; set; } = ThingType.None;

        /// <summary>
        /// Gets or sets the applicable Definition names which this relates to.
        /// If empty, any Definition is allowed.
        /// </summary>
        public List<string> Definitions { get; set; } = new List<string>();
    }
}

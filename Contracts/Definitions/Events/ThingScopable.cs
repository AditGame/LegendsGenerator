// <copyright file="ThingScopable.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;

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
        public string[] Definitions { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the condition on the thing to scope on.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string Condition { get; set; } = "UNSET";
    }
}

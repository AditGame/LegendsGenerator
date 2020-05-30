// <copyright file="Effect.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The effect of an Event on a Thing.
    /// </summary>
    public class Effect
    {
        /// <summary>
        /// Gets or sets a short title of the Effect.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a long description of the Effect.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Attribute which this Effect will modify.
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Gets or sets the effect this Effect has on the Attribute.
        /// </summary>
        public int AttributeEffect { get; set; }

        /// <summary>
        /// Gets or sets the Step this Effect was applied.
        /// </summary>
        public int TookEffect { get; set; }

        /// <summary>
        /// Gets or sets the Duration of this Effect.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets what Thing applied this Effect, if applicable.
        /// </summary>
        public Guid? AppliedBy { get; set; }
    }
}

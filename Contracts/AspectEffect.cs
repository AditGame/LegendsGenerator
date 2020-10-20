// <copyright file="AspectEffect.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    /// <summary>
    /// An effect applied to an Aspect.
    /// </summary>
    public record AspectEffect : BaseEffect
    {
        /// <summary>
        /// Gets or sets the Aspect which this Effect will modify.
        /// </summary>
        public string Aspect { get; set; } = "UNSET";

        /// <summary>
        /// Gets or sets the new value this Effect has on the Aspect.
        /// </summary>
        public string Value { get; set; } = "UNSET";
    }
}

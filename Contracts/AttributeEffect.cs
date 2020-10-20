// <copyright file="AttributeEffect.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts
{
    /// <summary>
    /// An effect applied to an attribute.
    /// </summary>
    public record AttributeEffect : BaseEffect
    {
        /// <summary>
        /// Gets or sets the Attribute which this Effect will modify.
        /// </summary>
        public string Attribute { get; set; } = "UNSET";

        /// <summary>
        /// Gets or sets the effect this Effect has on the Attribute.
        /// </summary>
        public int Manitude { get; set; }
    }
}

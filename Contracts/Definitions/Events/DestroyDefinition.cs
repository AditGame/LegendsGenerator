// <copyright file="DestroyDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// A definition of a result which destroys something
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class DestroyDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the description of this spawn result.
        /// </summary>
        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Description { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the thing which is to be destroyed.
        /// </summary>
        public string ThingDestroyed { get; set; } = UnsetString;
    }
}

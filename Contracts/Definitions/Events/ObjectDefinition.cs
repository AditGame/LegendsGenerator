// <copyright file="ObjectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

using LegendsGenerator.Contracts.Compiler;

namespace LegendsGenerator.Contracts.Definitions.Events
{
    /// <summary>
    /// The definition of an object in an event.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class ObjectDefinition : ThingScopable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this object should be optional.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Gets or sets the taxicab distance from the subject to consider objects.
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or sets the condition on the thing to scope on.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string Condition { get; set; } = "true";
    }
}

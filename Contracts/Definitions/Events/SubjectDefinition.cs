// <copyright file="SubjectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{

    using LegendsGenerator.Contracts.Compiler;

    public partial class SubjectDefinition : ThingScopable
    {
        /// <summary>
        /// Gets or sets the condition on the thing to scope on.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string Condition { get; set; } = "true";
    }
}

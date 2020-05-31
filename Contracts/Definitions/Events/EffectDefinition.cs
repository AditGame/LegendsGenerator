// <copyright file="EffectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using LegendsGenerator.Contracts.Compiler;

    [UsesAdditionalParametersForHoldingClass]
    public partial class EffectDefinition : BaseDefinition
    {
        public string AffectedAttribute { get; set; }

        [Compiled(typeof(int), "Subject")]
        public string Magnitude { get; set; }

        [Compiled(typeof(int), "Subject")]
        public string Duration { get; set; } = "-1";

        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Title { get; set; } = "UNDEFINED_TITLE";

        [Compiled(typeof(string), "Subject", AsFormattedText = true)]
        public string Description { get; set; } = "UNDEFINED_DESCRIPTION";
    }
}

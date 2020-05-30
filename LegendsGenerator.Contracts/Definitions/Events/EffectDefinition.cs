// <copyright file="EffectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EffectDefinition
    {
        public string AffectedAttribute { get; set; }

        public string Magnitude { get; set; }

        public string Duration { get; set; } = "-1";

        public string Title { get; set; }

        public string Description { get; set; }
    }
}

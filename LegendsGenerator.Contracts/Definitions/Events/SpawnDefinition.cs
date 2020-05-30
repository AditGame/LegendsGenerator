// <copyright file="SpawnDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SpawnDefinition
    {
        public ThingType Type { get; set; }

        public string DefinitionName { get; set; }

        public Dictionary<string, string> AttributeOverrides { get; } = new Dictionary<string, string>();

        public string Description { get; set; }
    }
}

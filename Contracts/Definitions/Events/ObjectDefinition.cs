// <copyright file="ObjectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    public partial class ObjectDefinition : ThingScopable
    {
        public bool Optional { get; set; }

        public int Distance { get; set; }
    }
}

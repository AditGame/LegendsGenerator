// <copyright file="ObjectDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public partial class ObjectDefinition : ThingScopable
    {
        public string VariableName { get; set; }

        public bool Optional { get; set; }

        public int Distance { get; set; }
    }
}

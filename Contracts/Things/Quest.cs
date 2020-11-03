// <copyright file="Quest.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Represents a quest!
    /// </summary>
    public record Quest : BaseThing
    {
        public Quest(QuestDefinition definition)
            : base(definition)
        {
        }

        public override ThingType ThingType => ThingType.Quest;
    }
}

// <copyright file="Quest.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Represents a quest!.
    /// </summary>
    public record Quest : BaseThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quest"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        public Quest(QuestDefinition definition)
            : base(definition)
        {
        }

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Quest;
    }
}

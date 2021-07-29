// <copyright file="Quest.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
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
        /// <param name="rdm">Random number generator.</param>
        public Quest(QuestDefinition definition, Random rdm)
            : base(definition, rdm)
        {
        }

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.Quest;

        /// <summary>
        /// Gets or sets the thing this is in.
        /// </summary>
        public Guid InThing { get; set; }
    }
}

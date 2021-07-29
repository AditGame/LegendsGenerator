// -------------------------------------------------------------------------------------------------
// <copyright file="BasePhysicalThingPres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Presentation of a physical thing.
    /// </summary>
    public class BasePhysicalThingPres : BaseThingPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePhysicalThingPres"/> class.
        /// </summary>
        /// <param name="inner">The inner object.</param>
        /// <param name="world">The world.</param>
        public BasePhysicalThingPres(BasePhysicalThing inner, World world)
            : base(inner, world)
        {
        }

        /// <summary>
        /// Gets the position of this thing.
        /// </summary>
        public PointPres Position => new PointPres(this.Inner.X, this.Inner.Y);

        /// <summary>
        /// Gets the square this thing is on.
        /// </summary>
        public WorldSquarePres WorldSquare => new WorldSquarePres(this.World.Grid.GetSquare(this.Position.X, this.Position.Y), this.World);

        /// <summary>
        /// Gets the quests.
        /// </summary>
        public IEnumerable<QuestPres> Quests => this.Inner.Quests.Select(q => new QuestPres(q, this.World));

        /// <inheritdoc/>
        protected override BasePhysicalThing Inner => (BasePhysicalThing)base.Inner;

        /// <summary>
        /// Gets a value indicating if the thing is in the specified quest.
        /// </summary>
        /// <param name="questDefinitionName">The quest definition name.</param>
        /// <returns>True if has the quest.</returns>
        public bool HasQuest(string questDefinitionName) =>
            this.Inner.Quests.Any(x => x.BaseDefinition.DefinitionName.Equals(questDefinitionName, StringComparison.OrdinalIgnoreCase));
    }
}

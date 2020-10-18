// <copyright file="WorldSquareDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Definition of a world square.
    /// </summary>
    public partial class WorldSquareDefinition : BaseThingDefinition
    {
        /// <summary>
        /// Gets or sets the cost of moving into this square.
        /// </summary>
        [Compiled(typeof(float))]
        [CompiledVariable("Subject", typeof(WorldSquare))]
        public string MovementCost { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets a value indicating whether this world square is considered water, mostly for movement purposes.
        /// </summary>
        public bool IsWater { get; set; }

        public bool IsSaltWater { get; set; }

        public int MinElevation { get; set; }

        public int MaxElevation { get; set; }

        public int MinRainfall { get; set; }

        public int MaxRainfall { get; set; }

        public int MinDrainage { get; set; }

        public int MaxDrainage { get; set; }

        public override ThingType ThingType => ThingType.WorldSquare;
    }
}

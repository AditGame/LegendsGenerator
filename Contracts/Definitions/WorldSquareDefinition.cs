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
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;
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
        [EditorIcon(EditorIcon.CompiledDynamic)]
        public string MovementCost { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets a value indicating whether this world square is considered water, mostly for movement purposes.
        /// </summary>
        public bool IsWater { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is salt water.
        /// </summary>
        public bool IsSaltWater { get; set; }

        /// <summary>
        /// Gets or sets the min elevation this square can be in, inclusive.
        /// </summary>
        public int MinElevation { get; set; }

        /// <summary>
        /// Gets or sets the max elevation this square can be in, inclusive.
        /// </summary>
        public int MaxElevation { get; set; }

        /// <summary>
        /// Gets or sets the min rainfall this square can be in, inclusive.
        /// </summary>
        public int MinRainfall { get; set; }

        /// <summary>
        /// Gets or sets the max rainfall this square can be in, inclusive.
        /// </summary>
        public int MaxRainfall { get; set; }

        /// <summary>
        /// Gets or sets the min drainage this square can be in, inclusive.
        /// </summary>
        public int MinDrainage { get; set; }

        /// <summary>
        /// Gets or sets the max drainage this square can be in, inclusive.
        /// </summary>
        public int MaxDrainage { get; set; }

        /// <inheritdoc/>
        public override ThingType ThingType => ThingType.WorldSquare;
    }
}

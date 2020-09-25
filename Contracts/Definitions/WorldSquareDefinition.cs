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

    /// <summary>
    /// Definition of a world square.
    /// </summary>
    public partial class WorldSquareDefinition : BaseThingDefinition
    {
        /// <summary>
        /// Gets or sets the cost of moving into this square.
        /// </summary>
        [Compiled(typeof(float), new[] { "Subject" })]
        public string MovementCost { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets a value indicating whether this world square is considered water, mostly for movement purposes.
        /// </summary>
        public bool IsWater { get; set; }
    }
}

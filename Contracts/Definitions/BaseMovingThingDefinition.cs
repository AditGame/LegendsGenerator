// <copyright file="BaseMovingThingDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// Base definition for things which MOVE!
    /// </summary>
    public partial class BaseMovingThingDefinition : BaseThingDefinition
    {
        /// <summary>
        /// Gets or sets a bool which represents if the unit can fly; if so they always use LandSpeed to move with no modifiers.
        /// </summary>
        [Compiled(typeof(bool), "Subject")]
        public string Flies { get; set; } = "false";

        /// <summary>
        /// Gets or sets an int which represents the speed of this unit in squares, when walking on land.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string LandSpeed { get; set; } = "1";

        /// <summary>
        /// Gets or sets an int which represents the speed of this unit in squares, when moving through water.
        /// </summary>
        [Compiled(typeof(int), "Subject")]
        public string WaterSpeed { get; set; } = "0";
    }
}

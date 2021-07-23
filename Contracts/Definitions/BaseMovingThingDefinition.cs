// <copyright file="BaseMovingThingDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Base definition for things which MOVE!.
    /// </summary>
    public abstract partial class BaseMovingThingDefinition : BaseThingDefinition
    {
        /// <summary>
        /// Gets or sets a bool which represents if the unit can fly; if so they always use LandSpeed to move with no modifiers.
        /// </summary>
        [Compiled(typeof(bool))]
        [CompiledVariable("Subject", typeof(BaseMovingThing))]
        [EditorIcon(EditorIcon.CompiledDynamic)]
        public string Flies { get; set; } = "false";

        /// <summary>
        /// Gets or sets an int which represents the speed of this unit in squares, when walking on land.
        /// </summary>
        [Compiled(typeof(float))]
        [CompiledVariable("Subject", typeof(BaseMovingThing))]
        [EditorIcon(EditorIcon.CompiledDynamic)]
        public string LandSpeed { get; set; } = "1";

        /// <summary>
        /// Gets or sets an int which represents the speed of this unit in squares, when moving through water.
        /// </summary>
        [Compiled(typeof(float))]
        [CompiledVariable("Subject", typeof(BaseMovingThing))]
        [EditorIcon(EditorIcon.CompiledDynamic)]
        public string WaterSpeed { get; set; } = "0";
    }
}

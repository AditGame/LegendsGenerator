// <copyright file="MoveDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// Definition of something moving to something else.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class MoveDefinition : BaseDefinition
    {
        /// <summary>
        /// Gets or sets the thing which will be doing te moving.
        /// </summary>
        public string ThingToMove { get; set; } = "Subject";

        /// <summary>
        /// Gets or sets the type of movement.
        /// </summary>
        public MoveType MoveType { get; set; }

        /// <summary>
        /// Gets or sets a thing this thing will move to.
        /// </summary>
        [HideInEditor("value.MoveType != MoveType.ToThing")]
        public string ThingToMoveTo { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the coord this thing will move towards.
        /// </summary>
        [HideInEditor("value.MoveType != MoveType.ToCoords")]
        [Compiled(typeof(int), "Subject")]
        public string CoordToMoveToX { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the coord this thing will move towards.
        /// </summary>
        [HideInEditorAttribute("value.MoveType != MoveType.ToCoords")]
        [Compiled(typeof(int), "Subject")]
        public string CoordToMoveToY { get; set; } = UnsetString;
    }
}

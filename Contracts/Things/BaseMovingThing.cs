// <copyright file="BaseMovingThing.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;
    using System.Collections.Immutable;
    using LegendsGenerator.Contracts.Definitions;

    /// <summary>
    /// A thing which can move on the world map.
    /// </summary>
    public abstract record BaseMovingThing : BasePhysicalThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMovingThing"/> class.
        /// </summary>
        /// <param name="definition">The thing definition.</param>
        protected BaseMovingThing(BaseThingDefinition definition)
            : base(definition)
        {
        }

        /// <summary>
        /// Gets the definition as a moving thing definition.
        /// </summary>
        public BaseMovingThingDefinition MovingThingDefinition =>
            this.BaseDefinition as BaseMovingThingDefinition ?? throw new InvalidOperationException("Unable to cast definition to MovingThing definition.");

        /// <summary>
        /// Gets or sets the type of movement expected.
        /// </summary>
        public MoveType MoveType { get; set; }

        /// <summary>
        /// Gets or sets the ID of a thing to travel towards. If null, this is not traveling towards anything.
        /// </summary>
        public Guid? MoveToThing { get; set; }

        /// <summary>
        /// Gets or sets the X coord to move towards.
        /// </summary>
        public int? MoveToCoordX { get; set; }

        /// <summary>
        /// Gets or sets the Y coord to move towards.
        /// </summary>
        public int? MoveToCoordY { get; set; }

        /// <summary>
        /// Gets or sets the amount of residual movement from the last movement.
        /// </summary>
        public float ResidualMovement { get; set; }

        /// <summary>
        /// Gets or sets the path to the final coord.
        /// </summary>
        public ImmutableList<Location>? Path { get; set; }

        /// <summary>
        /// Gets a value indicating whether this thing is moving towards something.
        /// </summary>
        public bool IsMoving => this.MoveType != MoveType.Unknown;

        /// <summary>
        /// Completes the movement and nulls out all values.
        /// </summary>
        public void CompleteMovement()
        {
            this.MoveType = MoveType.Unknown;
            this.Path = null;
            this.MoveToThing = null;
            this.MoveToCoordX = null;
            this.MoveToCoordY = null;
            this.ResidualMovement = 0;
        }

        /// <summary>
        /// Gets the destination of this thing.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <returns>The X and Y Coordinates.</returns>
        /// <exception cref="InvalidOperationException">This thing is not moving towards anything.</exception>
        public (int X, int Y) GetDestination(World world)
        {
            if (!this.IsMoving)
            {
                throw new InvalidOperationException("This thing is not moving towards anything.");
            }

            int x, y;
            switch (this.MoveType)
            {
                case MoveType.ToCoords:
                    x = this.MoveToCoordX ?? throw new InvalidOperationException("MoveType is set to ToCoords but MoveToCoordX is null.");
                    y = this.MoveToCoordY ?? throw new InvalidOperationException("MoveType is set to ToCoords but MoveToCoordY is null.");
                    break;
                case MoveType.ToThing:
                    BasePhysicalThing thing =
                        world.FindThing(this.MoveToThing ?? throw new InvalidOperationException("MoveType is set to ToThing but MoveToThing is null.")) as BasePhysicalThing ??
                        throw new InvalidOperationException($"Thing {this.MoveToThing} does not have a location.");
                    x = thing.X;
                    y = thing.Y;
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized move type {this.MoveType}");
            }

            return (x, y);
        }
    }
}

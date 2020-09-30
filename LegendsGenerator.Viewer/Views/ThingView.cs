// -------------------------------------------------------------------------------------------------
// <copyright file="ThingView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// A view into a thing.
    /// </summary>
    public class ThingView
    {
        /// <summary>
        /// The underlying thing.
        /// </summary>
        private readonly BaseThing thing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThingView"/> class.
        /// </summary>
        /// <param name="thing">The thing to view.</param>
        /// <param name="world">The world.</param>
        public ThingView(BaseThing thing, World world)
        {
            this.thing = thing;

            if (this.thing is BaseMovingThing moving && moving.IsMoving)
            {
                (this.MovingTowardsX, this.MovingTowardsY) = moving.GetDestination(world);
            }
        }

        /// <summary>
        /// Gets the name of the thing.
        /// </summary>
        public ThingType ThingType => this.thing.ThingType;

        /// <summary>
        /// Gets the name of the thing.
        /// </summary>
        public string Name => this.thing.Name;

        /// <summary>
        /// Gets the definition name of the thing.
        /// </summary>
        public string DefinitionName => this.thing.BaseDefinition.Name;

        /// <summary>
        /// Gets the definition description of the Thing.
        /// </summary>
        public string DefinitionDescription => this.thing.BaseDefinition.Description;

        /// <summary>
        /// Gets the ID of this Thing.
        /// </summary>
        public Guid ThingId => this.thing.ThingId;

        /// <summary>
        /// Gets the effects on the thing.
        /// </summary>
        public IList<EffectView> Effects => this.thing.Effects.Select(x => new EffectView(x)).ToList();

        /// <summary>
        /// Gets the base Attribute on this object before Effects are applied.
        /// </summary>
        public IList<Tuple<string, int>> BaseAttributes =>
            this.thing.BaseAttributes.Select(x => Tuple.Create(x.Key, x.Value)).ToList();

        /// <summary>
        /// Gets the base Attribute on this object after Effects are applied.
        /// </summary>
        public IList<AttributeView> Attributes =>
            this.thing.BaseAttributes.Keys.Select(x => new AttributeView(this.thing, x)).ToList();

        /// <summary>
        /// Gets a value indicating whether this thing is moving.
        /// </summary>
        public bool IsMoving => (this.thing as BaseMovingThing)?.IsMoving == true;

        /// <summary>
        /// Gets a string representing what this is moving towards.
        /// </summary>
        public string MovingTowards
        {
            get
            {
                if (!this.IsMoving || this.thing is not BaseMovingThing movingThing)
                {
                    return "Nothing";
                }

                return movingThing.MoveType switch
                {
                    MoveType.ToCoords => $"({movingThing.MoveToCoordX}, {movingThing.MoveToCoordY})",
                    MoveType.ToThing => movingThing.MoveToThing?.ToString() ?? "NullGuid",
                    _ => "Unknown",
                };
            }
        }

        /// <summary>
        /// Gets the X coord of the square this is moving towards.
        /// </summary>
        public int? MovingTowardsX { get; }

        /// <summary>
        /// Gets the Y coords of the square this is moving towards.
        /// </summary>
        public int? MovingTowardsY { get; }

        /// <summary>
        /// Gets the lines relevant to this Thing.
        /// </summary>
        public List<LineView> ReleventLines
        {
            get
            {
                List<LineView> lines = new List<LineView>();
                if (this.MovingTowardsX != null && this.MovingTowardsY != null)
                {
                    lines.Add(new LineView(this.thing.X, this.thing.Y, this.MovingTowardsX.Value, this.MovingTowardsY.Value));
                }

                return lines;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not ThingView view)
            {
                return false;
            }

            return this.ThingId == view.ThingId;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.ThingId.GetHashCode();
        }
    }
}

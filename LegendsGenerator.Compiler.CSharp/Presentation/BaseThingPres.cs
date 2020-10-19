﻿// -------------------------------------------------------------------------------------------------
// <copyright file="BaseThingPres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Presentation for BaseThing.
    /// </summary>
    public class BaseThingPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseThingPres"/> class.
        /// </summary>
        /// <param name="inner">The thing this represents.</param>
        /// <param name="world">The world.</param>
        public BaseThingPres(BaseThing inner, World world)
        {
            this.Inner = inner;
            this.World = world;
        }

        /// <summary>
        /// Gets the type this thing is.
        /// </summary>
        public ThingType ThingType => this.Inner.ThingType;

        /// <summary>
        /// Gets the name of this thing.
        /// </summary>
        public string Name => this.Inner.Name;

        /// <summary>
        /// Gets the thing ID.
        /// </summary>
        public Guid ThingId => this.Inner.ThingId;

        /// <summary>
        /// Gets the position of this thing.
        /// </summary>
        public PointPres Position => new PointPres(this.Inner.X, this.Inner.Y);

        /// <summary>
        /// Gets the base attribtues.
        /// </summary>
        public IReadOnlyDictionary<string, int> BaseAttributes => new ReadOnlyDictionary<string, int>(this.Inner.BaseAttributes);

        /// <summary>
        /// Gets the effects on this object.
        /// </summary>
        public IReadOnlyList<Effect> Effects => new ReadOnlyCollection<Effect>(this.Inner.Effects);

        /// <summary>
        /// Gets the square this thing is on.
        /// </summary>
        public WorldSquarePres WorldSquare => new WorldSquarePres(this.World.Grid.GetSquare(this.Position.X, this.Position.Y), this.World);

        /// <summary>
        /// Gets the inner object.
        /// </summary>
        protected virtual BaseThing Inner { get; }

        /// <summary>
        /// Gets the world.
        /// </summary>
        protected World World { get; }

        /// <summary>
        /// Gets the effects which are modifying the specified value.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <returns>The list of all effects which are modifying this specified attrbute.</returns>
        public IEnumerable<Effect> GetEffectsModifying(string attribute) => this.Inner.GetEffectsModifying(attribute);

        /// <summary>
        /// Calculates the effective attribute value based on the current effects.
        /// </summary>
        /// <param name="attribute">The attribute to get.</param>
        /// <param name="defaultValue">The default value, if the attribtue does not exist.</param>
        /// <returns>The effective attribute value.</returns>
        public int EffectiveAttribute(string attribute, int defaultValue) => this.Inner.EffectiveAttribute(attribute, defaultValue);
    }
}
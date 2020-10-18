// -------------------------------------------------------------------------------------------------
// <copyright file="WorldSquarePres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Definitions.Events;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The world square.
    /// </summary>
    public class WorldSquarePres : BaseThingPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldSquarePres"/> class.
        /// </summary>
        /// <param name="inner">The inner square.</param>
        /// <param name="world">The world.</param>
        public WorldSquarePres(GridSquare inner, World world)
            : base(inner.SquareDefinition ?? throw new InvalidOperationException("GridSquare must have a SquareDefinition to be loaded into the presentation layer."), world)
        {
            this.Square = inner;
        }

        /// <summary>
        /// Gets the things in the square.
        /// </summary>
        public IReadOnlyCollection<BaseThingPres> ThingsInSquare => this.ThingsInSqaureOfType<BaseThingPres>();

        /// <summary>
        /// Gets the sites in the square.
        /// </summary>
        public IReadOnlyCollection<SitePres> SitesInSquare => this.ThingsInSqaureOfType<SitePres>();

        /// <summary>
        /// Gets the moving things in the square.
        /// </summary>
        public IReadOnlyCollection<BaseMovingThingPres> MovingThingsInSquare => this.ThingsInSqaureOfType<BaseMovingThingPres>();

        /// <summary>
        /// Gets a value indicating whether this square is water.
        /// </summary>
        public bool IsWater => this.Inner.Definition.IsWater;

        /// <inheritdoc/>
        protected override WorldSquare Inner => (WorldSquare)base.Inner;

        /// <summary>
        /// Gets the square this represents.
        /// </summary>
        protected GridSquare Square { get; }

        /// <summary>
        /// Gets the things in the square by underlying type.
        /// </summary>
        /// <typeparam name="TType">The type of the thing.</typeparam>
        /// <returns>All things in this square which match.</returns>
        public IReadOnlyCollection<TType> ThingsInSqaureOfType<TType>()
            where TType : BaseThingPres =>
            new ReadOnlyCollection<TType>(this.Square.GetThings(true).Select(x => PresentationConverters.ConvertToPresentationType(x, this.World)).OfType<TType>().ToList());
    }
}

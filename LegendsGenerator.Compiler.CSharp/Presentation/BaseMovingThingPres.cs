// -------------------------------------------------------------------------------------------------
// <copyright file="BaseMovingThingPres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Base moving thing presentation.
    /// </summary>
    public class BaseMovingThingPres : BasePhysicalThingPres
    {
        /// <summary>
        /// The cached destination point.
        /// </summary>
        private PointPres? cachedDestination;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMovingThingPres"/> class.
        /// </summary>
        /// <param name="inner">The inner object.</param>
        /// <param name="world">The world.</param>
        public BaseMovingThingPres(BaseMovingThing inner, World world)
            : base(inner, world)
        {
        }

        /// <summary>
        /// Gets the destination of this thing.
        /// </summary>
        public PointPres Destination
        {
            get
            {
                if (this.cachedDestination == null)
                {
                    this.cachedDestination = new PointPres(this.Inner.GetDestination(this.World));
                }

                return this.cachedDestination;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this thing is moving.
        /// </summary>
        public bool IsMoving => this.Inner.IsMoving;

        /// <inheritdoc/>
        protected override BaseMovingThing Inner => (BaseMovingThing)base.Inner;
    }
}

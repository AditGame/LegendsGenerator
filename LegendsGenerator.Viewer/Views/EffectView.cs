// -------------------------------------------------------------------------------------------------
// <copyright file="EffectView.cs" company="Tom Luppi">
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

    /// <summary>
    /// A view into an effect.
    /// </summary>
    public class EffectView
    {
        /// <summary>
        /// The underlying effect.
        /// </summary>
        private Effect effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectView"/> class.
        /// </summary>
        /// <param name="effect">The underlying effect.</param>
        public EffectView(Effect effect)
        {
            this.effect = effect;
        }

        /// <summary>
        /// Gets a short title of the Effect.
        /// </summary>
        public string Title => this.effect.Title;

        /// <summary>
        /// Gets a long description of the Effect.
        /// </summary>
        public string Description => this.effect.Description;

        /// <summary>
        /// Gets the Attribute which this Effect will modify.
        /// </summary>
        public string Attribute => this.effect.Attribute;

        /// <summary>
        /// Gets the effect this Effect has on the Attribute.
        /// </summary>
        public int AttributeEffect => this.effect.AttributeEffect;

        /// <summary>
        /// Gets the Step this Effect was applied.
        /// </summary>
        public int TookEffect => this.effect.TookEffect;

        /// <summary>
        /// Gets the Duration of this Effect.
        /// </summary>
        public int Duration => this.effect.Duration;

        /// <summary>
        /// Gets a string versino of the duration.
        /// </summary>
        public string DurationString => this.Duration == -1 ? "Never" : this.Duration.ToString();

        /// <summary>
        /// Gets the string version of the effect.
        /// </summary>
        public string EffectString => this.AttributeEffect > 0 ? $"{this.Attribute} +{this.AttributeEffect}" : $"{this.Attribute} {this.AttributeEffect}";

        /// <summary>
        /// Gets what Thing applied this Effect, if applicable.
        /// </summary>
        public Guid? AppliedBy => this.effect.AppliedBy;
    }
}

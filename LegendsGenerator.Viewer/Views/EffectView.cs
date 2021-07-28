// -------------------------------------------------------------------------------------------------
// <copyright file="EffectView.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Viewer.Views
{
    using System;
    using System.Globalization;
    using LegendsGenerator.Contracts;

    /// <summary>
    /// A view into an effect.
    /// </summary>
    public class EffectView
    {
        /// <summary>
        /// The underlying effect.
        /// </summary>
        private readonly BaseEffect effect;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectView"/> class.
        /// </summary>
        /// <param name="effect">The underlying effect.</param>
        public EffectView(BaseEffect effect)
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
        public string Attribute => (this.effect is AttributeEffect attr) ? attr.Attribute : ((AspectEffect)this.effect).Aspect;

        /// <summary>
        /// Gets the effect this Effect has on the Attribute.
        /// </summary>
        public string AttributeEffect => this.effect switch
        {
            AttributeEffect attr => attr.Manitude.ToString(CultureInfo.CurrentCulture),
            AspectEffect aspect => aspect.Value,
            _ => string.Empty,
        };

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
        public string DurationString => this.Duration == -1 ? "Never" : this.Duration.ToString(CultureInfo.CurrentCulture);

        /// <summary>
        /// Gets the string version of the effect.
        /// </summary>
        public string EffectString => this.effect switch
        {
            AttributeEffect attr => attr.Manitude > 0 ? $"{this.Attribute} +{this.AttributeEffect}" : $"{this.Attribute} {this.AttributeEffect}",
            AspectEffect aspect => aspect.Value,
            _ => string.Empty,
        };

        /// <summary>
        /// Gets what Thing applied this Effect, if applicable.
        /// </summary>
        public Guid? AppliedBy => this.effect.AppliedBy;
    }
}

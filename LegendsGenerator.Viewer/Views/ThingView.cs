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
        private BaseThing thing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThingView"/> class.
        /// </summary>
        /// <param name="thing">The thing to view.</param>
        public ThingView(BaseThing thing)
        {
            this.thing = thing;
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
    }
}

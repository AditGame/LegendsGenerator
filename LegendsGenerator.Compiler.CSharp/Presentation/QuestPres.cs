// -------------------------------------------------------------------------------------------------
// <copyright file="QuestPres.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp.Presentation
{
    using LegendsGenerator.Contracts;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Presentation of a quest.
    /// </summary>
    public class QuestPres : BaseThingPres
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestPres"/> class.
        /// </summary>
        /// <param name="inner">The inner thing.</param>
        /// <param name="world">The world.</param>
        public QuestPres(Quest inner, World world)
            : base(inner, world)
        {
        }

        /// <inheritdoc/>
        protected override Quest Inner => (Quest)base.Inner;
    }
}

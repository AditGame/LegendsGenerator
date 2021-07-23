// -------------------------------------------------------------------------------------------------
// <copyright file="EndQuestDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// Ends a quest.
    /// </summary>
    public partial class EndQuestDefinition : BaseEffectDefinition
    {
        /// <summary>
        /// Gets or sets the list of quests (by name) to end.
        /// </summary>
        [HideInEditor("value.AllQuests")]
        public Collection<string> Quests { get; set; } = new Collection<string>();

        /// <summary>
        /// Gets or sets a value indicating whether to end all quests.
        /// </summary>
        public bool AllQuests { get; set; }
    }
}

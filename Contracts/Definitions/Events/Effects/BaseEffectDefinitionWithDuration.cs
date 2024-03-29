﻿// <copyright file="BaseEffectDefinitionWithDuration.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// Base effect which has duration.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class BaseEffectDefinitionWithDuration : BaseEffectDefinition
    {
        /// <summary>
        /// Gets or sets the duration of the change. -1 is forever.
        /// </summary>
        [Compiled(typeof(int))]
        [CompiledVariable(EventDefinition.SubjectVarName, typeof(BaseThing))]
        public string Duration { get; set; } = UnsetString;
    }
}

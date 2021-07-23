// <copyright file="DestroyDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events.Effects
{
    using LegendsGenerator.Contracts.Compiler;

    /// <summary>
    /// A definition of a result which destroys something.
    /// </summary>
    [UsesAdditionalParametersForHoldingClass]
    public partial class DestroyDefinition : BaseEffectDefinition
    {
    }
}

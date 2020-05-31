// <copyright file="UsesAdditionalParametersForHoldingClassAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;

    /// <summary>
    /// If set on a class, the AdditionalParametersForClass on any holding class will be used in this class as well.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UsesAdditionalParametersForHoldingClassAttribute : Attribute
    {
    }
}

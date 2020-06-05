// -------------------------------------------------------------------------------------------------
// <copyright file="IntDefinitionNode.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System.Collections.Generic;
    using System.Reflection;

    public class IntDefinitionNode : DefinitionNode
    {
        public IntDefinitionNode(object? thing, ElementInfo property, IEnumerable<PropertyInfo> options, bool readOnly = false)
            : base(thing, property, options, readOnly)
        {
        }
    }
}

// -------------------------------------------------------------------------------------------------
// <copyright file="IEditorIntegration.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Compiler.CSharp
{
    using LegendsGenerator.Contracts.Compiler.EditorIntegration;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Integration for the editor from the compiler info.
    /// </summary>
    public interface IEditorIntegration
    {
        /// <summary>
        /// Pseudo intellisense for a type.
        /// </summary>
        /// <param name="type">The type to get public members for.</param>
        /// <returns>The string representation of all public members.</returns>
        IList<BaseTypeMember> GetPublicMembers(Type type);
    }
}
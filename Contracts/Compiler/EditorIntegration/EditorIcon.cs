// <copyright file="EditorIcon.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler.EditorIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The different editor icons.
    /// </summary>
    public enum EditorIcon
    {
        /// <summary>
        /// No icon, displays nothing.
        /// </summary>
        None,

        /// <summary>
        /// A one-timed run condition.
        /// </summary>
        CompiledStatic,

        /// <summary>
        /// A per-step run condition.
        /// </summary>
        CompiledDynamic,
    }
}

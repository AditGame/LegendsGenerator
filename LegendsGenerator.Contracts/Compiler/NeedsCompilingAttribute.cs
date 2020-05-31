// <copyright file="NeedsCompilingAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute which indicating the class needs compiling.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NeedsCompilingAttribute : Attribute
    {
    }
}

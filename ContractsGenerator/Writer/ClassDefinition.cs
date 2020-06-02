// <copyright file="ClassDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.ContractsGenerator.Writer
{
    using System.Collections.Generic;

    /// <summary>
    /// The definition of a class.
    /// </summary>
    public class ClassDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassDefinition"/> class.
        /// </summary>
        /// <param name="nameSpace">The namespace of the class.</param>
        /// <param name="className">The class name to generate.</param>
        public ClassDefinition(
            string nameSpace,
            string className)
        {
            this.NameSpace = nameSpace;
            this.ClassName = className;
        }

        /// <summary>
        /// Gets the namespace this class should be in.
        /// </summary>
        public string NameSpace { get; }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Gets or sets the access level of this class.
        /// </summary>
        public AccessLevel Access { get; set; } = AccessLevel.Public;

        /// <summary>
        /// Gets or sets a value indicating whether this class is a partial class.
        /// </summary>
        public bool Partial { get; set; }

        /// <summary>
        /// Gets or sets the assemblies required by this class.
        /// </summary>
        public IList<string> Usings { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets whether to enable Nullable on this class.
        /// </summary>
        public bool Nullable { get; set; }
    }
}

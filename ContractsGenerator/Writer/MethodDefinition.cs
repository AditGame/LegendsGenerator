// <copyright file="MethodDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.ContractsGenerator.Writer
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// the definition of a method.
    /// </summary>
    public class MethodDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        public MethodDefinition(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the access level of the method.
        /// </summary>
        public AccessLevel Access { get; set; } = AccessLevel.Public;

        /// <summary>
        /// Gets or sets the return type of the method.
        /// </summary>
        public string Type { get; set; } = "void";

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the summary doc of the method, for XML documentation. Ignored if this method is set to override.
        /// </summary>
        public string? SummaryDoc { get; set; }

        /// <summary>
        /// Gets or sets the returns doc of the method, for XML documentation. Ignored if this method is set to override.
        /// </summary>
        public string? ReturnsDoc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this method should hide a base method.
        /// </summary>
        public bool New { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this method should override a base method.
        /// </summary>
        public bool Override { get; set; }

        /// <summary>
        /// Gets or sets all parameters of this method.
        /// </summary>
        public IEnumerable<ParamDef> Parameters { get; set; } = System.Array.Empty<ParamDef>();

        /// <summary>
        /// Adds the method header to the class writer.
        /// </summary>
        /// <param name="cw">The class writer.</param>
        public void ToString(ClassWriter cw)
        {
            string newString = this.New ? "new " : string.Empty;
            string overrideString = this.Override ? "override " : string.Empty;
            string paramList = string.Join(", ", this.Parameters.Select(p =>
                {
                    string def = string.Empty;
                    if (p.DefaultValue != null)
                    {
                        def = $" = {p.DefaultValue}";
                    }

                    return $"{p.Type} {p.Name}{def}";
                }));

            if (this.Override)
            {
                cw.AppendLine("/// <inheritdoc/>");
            }
            else
            {
#pragma warning disable CA1508 // Avoid dead conditional code. False positive.
                if (!string.IsNullOrWhiteSpace(this.SummaryDoc) && this.SummaryDoc != null)
#pragma warning restore CA1508 // Avoid dead conditional code
                {
                    cw.SummaryDoc(this.SummaryDoc);
                }

                foreach (var param in this.Parameters)
                {
                    cw.AppendLine($@"/// <param name=""{param.Name}"">{param.Description}</param>");
                }

                if (!string.IsNullOrWhiteSpace(this.ReturnsDoc))
                {
                    cw.AppendLine($"/// <returns>{this.ReturnsDoc}</returns>");
                }
            }

            cw.AppendLine($"{this.Access.AccessString()} {newString}{overrideString}{this.Type} {this.Name}({paramList})");
        }
    }
}

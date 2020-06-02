// <copyright file="ParamDef.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.ContractsGenerator.Writer
{
    /// <summary>
    /// The definition of a parameter.
    /// </summary>
    public class ParamDef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParamDef"/> class.
        /// </summary>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="description">The description, for XML documentation.</param>
        public ParamDef(string type, string name, string description)
        {
            this.Type = type;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamDef"/> class.
        /// </summary>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="defaultValue">The default value of the parameter.</param>
        /// <param name="description">The description, for XML documentation.</param>
        public ParamDef(string type, string name, string defaultValue, string description)
        {
            this.Type = type;
            this.Name = name;
            this.DefaultValue = defaultValue;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the parameter name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the parameter, for XML documentation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets an optional default value of the parameter.
        /// </summary>
        public string? DefaultValue { get; set; }
    }
}

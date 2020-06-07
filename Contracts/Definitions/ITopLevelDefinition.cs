// <copyright file="ITopLevelDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// A definition at the top of the serialization stack.
    /// </summary>
    public interface ITopLevelDefinition
    {
        /// <summary>
        /// Gets or sets the file this definition was sourced from. Used for the editor.
        /// </summary>
        [JsonIgnore]
        string SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the name of the definition. Used in error logs or the editor.
        /// </summary>
        string DefinitionName { get; set; }
    }
}

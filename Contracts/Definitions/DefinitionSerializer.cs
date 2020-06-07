// <copyright file="DefinitionSerializer.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Encodings.Web;
    using System.Text.Json;

    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Serializes and deserializes the definitions.
    /// </summary>
    public static class DefinitionSerializer
    {
        /// <summary>
        /// The options for JSON serialization.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        /// <summary>
        /// Deserializes all definitions and events from the specified directory.
        /// </summary>
        /// <param name="path">The path to the directory.</param>
        /// <returns>A tuple of definitions and events from the directory.</returns>
        public static DefinitionCollection DeserializeFromDirectory(string path)
        {
            List<BaseDefinition> defs = new List<BaseDefinition>();
            foreach (string file in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
            {
                string json = File.ReadAllText(file);
                DefinitionFile? deserializedFile = JsonSerializer.Deserialize<DefinitionFile>(json, JsonOptions);

                if (deserializedFile != null)
                {
                    foreach (BaseDefinition eve in deserializedFile.AllDefinitions())
                    {
                        if (eve is ITopLevelDefinition toplevel)
                        {
                            toplevel.SourceFile = file;
                        }

                        defs.Add(eve);
                    }
                }
            }

            return new DefinitionCollection(defs);
        }

        /// <summary>
        /// Reserializes all definitions to files based on the SoureFile property.
        /// </summary>
        /// <param name="definitions">The definitions.</param>
        public static void ReserializeToFiles(DefinitionCollection definitions)
        {
            IEnumerable<IGrouping<string, ITopLevelDefinition>> byFile = 
                definitions.AllDefinitions.OfType<ITopLevelDefinition>().GroupBy(d => d.SourceFile);

            // Alert early if any definition is invalid.
            IGrouping<string, ITopLevelDefinition>? unsetGroup = byFile.FirstOrDefault(x => x.Key.Equals(BaseDefinition.UnsetString));
            if (unsetGroup != null)
            {
                throw new InvalidOperationException(
                    $"The following definitions have unset source files: {string.Join(", ", unsetGroup.Select(d => d.DefinitionName))}");
            }

            foreach (IGrouping<string, BaseDefinition> file in byFile)
            {
                SerializeToFile(new DefinitionCollection(file), file.Key);
            }
        }

        /// <summary>
        /// Serializes the specified deinitions and events to a file.
        /// </summary>
        /// <param name="definitions">The definitions to serialize.</param>
        /// <param name="filename">The filename to serialize to.</param>
        public static void SerializeToFile(DefinitionCollection definitions, string filename)
        {
            var definitionFile = new DefinitionFile(definitions);
            string serialized = JsonSerializer.Serialize(definitionFile, JsonOptions);
            File.WriteAllText(filename, serialized);
        }
    }
}

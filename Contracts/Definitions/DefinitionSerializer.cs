// <copyright file="DefinitionSerializer.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
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
        public static(DefinitionCollections definitions, EventCollection events) DeserializeFromDirectory(string path)
        {
            IList<SiteDefinition> sites = new List<SiteDefinition>();
            IList<EventDefinition> events = new List<EventDefinition>();
            foreach (string file in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
            {
                string json = File.ReadAllText(file);
                DefinitionFile? deserializedFile = JsonSerializer.Deserialize<DefinitionFile>(json, JsonOptions);

                if (deserializedFile != null)
                {
                    deserializedFile.Events.ToList().ForEach(e => events.Add(e));
                    deserializedFile.Sites.ToList().ForEach(t => sites.Add(t));
                }
            }

            return (new DefinitionCollections(sites), new EventCollection(events));
        }

        /// <summary>
        /// Serializes the specified deinitions and events to a file.
        /// </summary>
        /// <param name="definitions">The definitions to serialize.</param>
        /// <param name="events">The events to serialize.</param>
        /// <param name="filename">The filename to serialize to.</param>
        public static void SerializeToFile(DefinitionCollections definitions, EventCollection events, string filename)
        {
            var definitionFile = new DefinitionFile(definitions, events);
            string serialized = JsonSerializer.Serialize(definitionFile, JsonOptions);
            File.WriteAllText(filename, serialized);
        }
    }
}

// <copyright file="DefinitionSerializer.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// Serializes and deserializes the definitions.
    /// </summary>
    public static class DefinitionSerializer
    {
        /// <summary>
        /// The options for JSON serialization.
        /// </summary>
        /// <remarks>Initialized in static constructor.</remarks>
        private static readonly JsonSerializerOptions JsonOptions;

        /// <summary>
        /// Initializes static members of the <see cref="DefinitionSerializer"/> class.
        /// </summary>
        static DefinitionSerializer()
        {
            JsonOptions = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                ReadCommentHandling = JsonCommentHandling.Skip,
            };

            // The Editor will have null entries in lists so this will remove them at serialization time.
            JsonOptions.Converters.Add(new NullListEntryConverterFactory());
        }

        /// <summary>
        /// Deserializes all definitions and events from the specified directory.
        /// </summary>
        /// <param name="compiler">The condition compiler to use.</param>
        /// <param name="path">The path to the directory.</param>
        /// <returns>A tuple of definitions and events from the directory.</returns>
        public static DefinitionCollection DeserializeFromDirectory(IConditionCompiler compiler, string path)
        {
            List<BaseDefinition> defs = new List<BaseDefinition>();
            foreach (string file in Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories))
            {
                string json = File.ReadAllText(file);
                DefinitionFile? deserializedFile = JsonSerializer.Deserialize<DefinitionFile>(json, JsonOptions);

                if (deserializedFile != null)
                {
                    foreach (BaseDefinition eve in deserializedFile.AllDefinitions)
                    {
                        if (eve is ITopLevelDefinition toplevel)
                        {
                            toplevel.SourceFile = file;
                        }

                        defs.Add(eve);
                    }
                }
            }

            var definitions = new DefinitionCollection(defs);
            definitions.Attach(compiler);
            return definitions;
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

            foreach (IGrouping<string, ITopLevelDefinition> file in byFile)
            {
                SerializeToFile(new DefinitionCollection(file.OfType<BaseDefinition>()), file.Key);
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

        /// <summary>
        /// Serializes lists, omitting null entries.
        /// </summary>
        private class NullListEntryConverterFactory : JsonConverterFactory
        {
            /// <inheritdoc/>
            public override bool CanConvert(Type typeToConvert)
            {
                if (!typeToConvert.IsGenericType)
                {
                    return false;
                }

                return typeToConvert.GetGenericTypeDefinition() == typeof(List<>);
            }

            /// <inheritdoc/>
            public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            {
                Type valueType = typeToConvert.GetGenericArguments()[0];

                JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                    typeof(NullListEntryConverter<>).MakeGenericType(
                        new Type[] { valueType }),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { options },
                    culture: null)!;

                return converter;
            }

            /// <summary>
            /// The inner converter type for lists which have null entries.
            /// </summary>
            /// <typeparam name="T">The list value type.</typeparam>
            private class NullListEntryConverter<T> : JsonConverter<List<T>>
            {
                /// <summary>
                /// The value converter for the list elements.
                /// </summary>
                private readonly JsonConverter<T>? valueConverter;

                /// <summary>
                /// Initializes a new instance of the <see cref="NullListEntryConverter{T}"/> class.
                /// </summary>
                /// <param name="options">The josn converter options.</param>
                public NullListEntryConverter(JsonSerializerOptions options)
                {
                    // Use the existing converters if available.
                    this.valueConverter = (JsonConverter<T>)options
                        .GetConverter(typeof(T));
                }

                /// <inheritdoc/>
                [return: MaybeNull]
                public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    return JsonSerializer.Deserialize<List<T>>(ref reader);
                }

                /// <inheritdoc/>
                public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
                {
                    writer.WriteStartArray();

                    foreach (T entry in value)
                    {
                        if (entry == null)
                        {
                            // The whole point of this, skip null entries.
                            continue;
                        }

                        if (this.valueConverter != null)
                        {
                            this.valueConverter.Write(writer, entry, options);
                        }
                        else
                        {
                            JsonSerializer.Serialize(writer, entry, options);
                        }
                    }

                    writer.WriteEndArray();
                }
            }
        }
    }
}

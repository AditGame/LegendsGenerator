// <copyright file="EventDefinition.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Definitions.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions.Validation;
    using LegendsGenerator.Contracts.Things;

    /// <summary>
    /// The definition of a single event.
    /// </summary>
    public partial class EventDefinition : BaseDefinition, ITopLevelDefinition
    {
        /// <summary>
        /// The name of the Subject variable name.
        /// </summary>
        public const string SubjectVarName = "Subject";

        /// <inheritdoc/>
        [JsonIgnore]
        public string SourceFile { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the definition name, if not set the definition name will be the event description.
        /// </summary>
        [ControlsDefinitionName]
        public string? DefinitionName { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        string ITopLevelDefinition.DefinitionName
        {
            get
            {
                if (string.IsNullOrEmpty(this.DefinitionName))
                {
                    return this.Description;
                }
                else
                {
                    return this.DefinitionName;
                }
            }

            set
            {
                this.DefinitionName = value;
            }
        }

        /// <summary>
        /// Gets or sets the event Condition, from one to one hundred.
        /// </summary>
        [Compiled(typeof(int))]
        [CompiledVariable(SubjectVarName, typeof(BaseThing))]
        public string Chance { get; set; } = "100";

        /// <summary>
        /// Gets or sets a value indicating whether this event can trigger while the Thing is moving.
        /// </summary>
        public bool CanTriggerWhileMoving { get; set; }

        /// <summary>
        /// Gets or sets the affect this event has on movement.
        /// </summary>
        [HideInEditor("value.CanTriggerWhileMoving != true")]
        public AffectsMovement AffectOnMovement { get; set; }

        /// <summary>
        /// Gets or sets the subject of this event.
        /// </summary>
        public SubjectDefinition Subject { get; set; } = new SubjectDefinition();

        /// <summary>
        /// Gets or sets the objects of this event.
        /// </summary>
        public Dictionary<string, ObjectDefinition> Objects { get; set; } = new Dictionary<string, ObjectDefinition>();

        /// <summary>
        /// Gets or sets the description of this event.
        /// </summary>
        [Compiled(typeof(string), AsFormattedText = true)]
        [CompiledVariable(SubjectVarName, typeof(BaseThing))]
        [ControlsDefinitionName]
        public string Description { get; set; } = UnsetString;

        /// <summary>
        /// Gets or sets the results of this event.
        /// </summary>
        public List<EventResultDefinition> Results { get; set; } = new List<EventResultDefinition>();

        /// <summary>
        /// Gets additional variable names for the Description method.
        /// </summary>
        /// <returns>The list of additional parameters.</returns>
        public IList<CompiledVariable> AdditionalParametersForClass()
        {
            return this.Objects?.Select(x => new CompiledVariable(x.Key, x.Value.Type.AssociatedType())).ToList() ?? new List<CompiledVariable>();
        }

        /// <summary>
        /// Gets the actual type of the Subject variable.
        /// </summary>
        /// <returns>The  actual type of the subject variable.</returns>
        public Type TypeOfSubject()
        {
            return this.Subject.Type.AssociatedType();
        }
    }
}

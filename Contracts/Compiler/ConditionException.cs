using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Compiler
{
    [Serializable]
    public class ConditionException : Exception
    {
        public ConditionException()
        {
        }

        public ConditionException(string? message) : base(message)
        {
        }

        public ConditionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public ConditionException(string defintion, string conditionName)
            : base($"Condition {defintion}::{conditionName} failed.")
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
        }

        public ConditionException(string defintion, string conditionName, Exception inner)
            : base($"Condition {defintion}::{conditionName} failed.", inner)
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
        }

        public ConditionException(string defintion, string conditionName, string error)
            : base($"Condition {defintion}::{conditionName} failed with {error}.")
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
            this.Error = error;
        }

        public ConditionException(string defintion, string conditionName, string error, Exception inner)
            : base($"Condition {defintion}::{conditionName} failed with {error}.", inner)
        {
            this.DefinitionName = defintion;
            this.ConditionName = conditionName;
            this.Error = error;
        }

        protected ConditionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.DefinitionName = info.GetString(nameof(this.DefinitionName));
            this.ConditionName = info.GetString(nameof(this.ConditionName));
            this.Error = info.GetString(nameof(this.Error));
        }

        public string? DefinitionName { get; }

        public string? ConditionName { get; }

        public string? Error { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(this.DefinitionName), this.DefinitionName);
            info.AddValue(nameof(this.ConditionName), this.ConditionName);
            info.AddValue(nameof(this.Error), this.Error);
        }
    }
}

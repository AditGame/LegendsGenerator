using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsGenerator.Contracts.Definitions.Validation
{
    /// <summary>
    /// The level of validation issue.
    /// </summary>
    public enum ValidationLevel
    {
        /// <summary>
        /// There is no issue.
        /// </summary>
        None,

        /// <summary>
        /// Potential improvement.
        /// </summary>
        Info,

        /// <summary>
        /// Potential issue.
        /// </summary>
        Warning,

        /// <summary>
        /// Issue.
        /// </summary>
        Error
    }
}

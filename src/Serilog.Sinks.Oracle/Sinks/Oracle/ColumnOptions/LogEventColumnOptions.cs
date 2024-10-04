using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the LogEvent column.
    /// </summary>
    public class LogEventColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LogEventColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.LogEvent;
            DataType = OracleDbType.NVarchar2;
        }

        /// <summary>
        /// The LogEvent column defaults to NVarChar and must be of a character-storage data type.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value))
                    throw new ArgumentException("The Standard Column \"LogEvent\" must be NVarChar or VarChar.");
                base.DataType = value;
            }
        }

        /// <summary>
        /// Exclude properties from the LogEvent column if they are being saved to additional columns.
        /// Defaults to false for backwards-compatibility, but true is the recommended setting.
        /// </summary>
        public bool ExcludeAdditionalProperties { get; set; }

        /// <summary>
        /// Whether to include Standard Columns in the LogEvent column (for backwards compatibility).
        /// Defaults to false for backwards-compatibility, but true is the recommended setting.
        /// </summary>
        public bool ExcludeStandardColumns { get; set; }
    }
}

using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;

public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the SpanId column.
    /// </summary>
    public class SpanIdColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SpanIdColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.SpanId;
            DataType = OracleDbType.Varchar2;
        }

        /// <summary>
        /// The SpanId column defaults to NVarChar and must be of a character-storage data type.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value))
                    throw new ArgumentException("The Standard Column \"SpanId\" must be NVarChar or VarChar.");
                base.DataType = value;
            }
        }
    }
}

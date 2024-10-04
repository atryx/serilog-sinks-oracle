using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the Exception column.
    /// </summary>
    public class ExceptionColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ExceptionColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.Exception;
            DataType = OracleDbType.NVarchar2;
        }

        /// <summary>
        /// The Exception column defaults to NVarChar and must be of a character-storage data type.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value))
                    throw new ArgumentException("The Standard Column \"Exception\" must be NVarChar or VarChar.");
                base.DataType = value;
            }
        }
    }
}

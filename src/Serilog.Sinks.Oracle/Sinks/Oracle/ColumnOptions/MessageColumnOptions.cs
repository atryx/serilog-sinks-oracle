using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the message column
    /// </summary>
    public class MessageColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.Message;
            DataType = OracleDbType.Clob;
        }

        /// <summary>
        /// The Message column defaults to NVarChar and must be of a character-storage data type.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value))
                    throw new ArgumentException("The Standard Column \"Message\" must be NVarChar or VarChar.");
                base.DataType = value;
            }
        }
    }
}

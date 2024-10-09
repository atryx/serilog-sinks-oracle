using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the MessageTemplate column.
    /// </summary>
    public class MessageTemplateColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MessageTemplateColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.MessageTemplate;
            DataType = OracleDbType.Clob;
        }

        /// <summary>
        /// The MessageTemplate column defaults to NVarChar and must be of a character-storage data type.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value))
                    throw new ArgumentException("The Standard Column \"MessageTemplate\" must be NVarChar or VarChar.");
                base.DataType = value;
            }
        }
    }
}

using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the Level column.
    /// </summary>
    public class LevelColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LevelColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.LogLevel;
            DataType = OracleDbType.Varchar2;
        }

        /// <summary>
        /// The Level column must be either NVarChar (the default) or TinyInt (which stores the underlying Level enum value).
        /// The recommended DataLength for NVarChar is 16 characters.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value) && value != OracleDbType.Byte)
                    throw new ArgumentException("The Standard Column \"Level\" must be of data type NVarChar, VarChar or TinyInt.");
                base.DataType = value;
            }
        }


        /// <summary>
        /// If true will store Level as an enum in a tinyint column as opposed to a string.
        /// </summary>
        public bool StoreAsEnum
        {
            get => (base.DataType == OracleDbType.Byte);
            set
            {
                base.DataType = value ? OracleDbType.Byte : OracleDbType.Varchar2;
            }
        }
    }
}

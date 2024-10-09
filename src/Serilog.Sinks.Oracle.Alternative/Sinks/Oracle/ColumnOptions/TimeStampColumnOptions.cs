using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the TimeStamp column.
    /// </summary>
    public class TimeStampColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TimeStampColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.TimeStamp;
            DataType = OracleDbType.TimeStamp;
        }

        /// <summary>
        /// The TimeStamp column only supports the DateTime, DateTime2 and DateTimeOffset data types.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (value != OracleDbType.TimeStamp && value != OracleDbType.TimeStampTZ && value != OracleDbType.TimeStampLTZ)
                    throw new ArgumentException("The Standard Column \"TimeStamp\" only supports the DateTime, DateTime2 and DateTimeOffset formats.");
                base.DataType = value;
            }
        }

        /// <summary>
        /// If true, the logging source's local time is converted to Coordinated Universal Time.
        /// By definition, UTC does not include any timezone or timezone offset information.
        /// </summary>
        public bool ConvertToUtc { get; set; }
    }
}

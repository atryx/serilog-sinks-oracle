﻿using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;

public partial class ColumnOptions // Standard Column options are inner classes for backwards-compatibility.
{
    /// <summary>
    /// Options for the TraceId column.
    /// </summary>
    public class TraceIdColumnOptions : OracleColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TraceIdColumnOptions() : base()
        {
            StandardColumnIdentifier = StandardColumn.TraceId;
            DataType = OracleDbType.NVarchar2;
        }

        /// <summary>
        /// The TraceId column defaults to NVarChar and must be of a character-storage data type.
        /// </summary>
        public new OracleDbType DataType
        {
            get => base.DataType;
            set
            {
                if (!OracleDataTypes.VariableCharacterColumnTypes.Contains(value))
                    throw new ArgumentException("The Standard Column \"TraceId\" must be NVarChar or VarChar.");
                base.DataType = value;
            }
        }
    }
}

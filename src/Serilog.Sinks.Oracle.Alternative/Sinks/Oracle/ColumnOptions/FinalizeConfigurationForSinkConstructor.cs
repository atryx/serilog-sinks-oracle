﻿using static System.FormattableString;

namespace Serilog.Sinks.Oracle
{
    public partial class ColumnOptions
    {
        private bool _configurationFinalized;

        /// <summary>
        /// The logging sink and audit sink constructors call this. Defaults are resolved (like ensuring the
        /// primary key is non-null) and obsolete features are migrated to their replacement features so
        /// dependencies in the sink itself can be safely removed as early as possible.
        /// </summary>
        internal void FinalizeConfigurationForSinkConstructor()
        {
            if (_configurationFinalized)
                return;

            // the constructor sets Id as the PK, remove it if the Id column was removed
            if (!Store.Contains(StandardColumn.Id) && PrimaryKey == Id)
                PrimaryKey = null;

            if (ClusteredColumnstoreIndex)
            {
                if (PrimaryKey != null)
                {
                    PrimaryKey = null;
                    SelfLog.WriteLine("Warning: Removing primary key, incompatible with clustered columnstore indexing.");
                }

                foreach (var stdcol in Store)
                    ColumnstoreCompatibilityCheck(GetStandardColumnOptions(stdcol));
            }

            if (AdditionalColumns != null)
            {
                foreach (var col in AdditionalColumns)
                {
                    if (string.IsNullOrWhiteSpace(col.ColumnName))
                        throw new ArgumentException("All custom columns must have a valid ColumnName property.");

                    // TODO: get another type to represent unsupoorted
                    //if (col.DataType == OracleDataTypes.NotSupported)
                    //    throw new ArgumentException(Invariant($"Column \"{col.ColumnName}\" specified an invalid or unsupported SQL column type."));

                    if (ClusteredColumnstoreIndex)
                        ColumnstoreCompatibilityCheck(col);
                }
            }

            // PK must always be NON-NULL
            if (PrimaryKey != null && PrimaryKey.AllowNull == true)
            {
                SelfLog.WriteLine("Warning: Primary key must be NON-NULL, changing AllowNull property for {0} column.", PrimaryKey.ColumnName);
                PrimaryKey.AllowNull = false;
            }

            _configurationFinalized = true;
        }

        private static void ColumnstoreCompatibilityCheck(OracleColumn column)
        {
            if (!OracleDataTypes.ColumnstoreCompatible.Contains(column.DataType))
                throw new ArgumentException(Invariant($"Columnstore indexes do not support data type \"{column.DataType}\" declared for column \"{column.ColumnName}\"."));

            if (column.DataLength == -1 && OracleDataTypes.DataLengthRequired.Contains(column.DataType))
                SelfLog.WriteLine("Warning: SQL2017 or newer required to use columnstore index with MAX length column \"{0}\".", column.ColumnName);
        }
    }
}

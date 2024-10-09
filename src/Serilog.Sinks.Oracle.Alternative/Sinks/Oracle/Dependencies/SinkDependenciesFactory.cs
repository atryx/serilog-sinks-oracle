using System;
using Serilog.Formatting;
using Serilog.Sinks.Oracle.Output;
using Serilog.Sinks.Oracle.Platform;
using Serilog.Sinks.Oracle.Platform.SqlClient;

namespace Serilog.Sinks.Oracle.Dependencies;

internal static class SinkDependenciesFactory
{
    internal static SinkDependencies Create(
        string connectionString,
        OracleSinkOptions sinkOptions,
        IFormatProvider formatProvider,
        ColumnOptions columnOptions,
        ITextFormatter logEventFormatter)
    {
        columnOptions = columnOptions ?? new ColumnOptions();
        columnOptions.FinalizeConfigurationForSinkConstructor();

        // Add 'Enlist=false', so that ambient transactions (TransactionScope) will not affect/rollback logging
        // unless sink option EnlistInTransaction is set to true.
        var sqlConnectionStringBuilderWrapper = new OracleConnectionStringBuilderWrapper(
            connectionString, sinkOptions.EnlistInTransaction);
        var sqlConnectionFactory = new SqlConnectionFactory(sqlConnectionStringBuilderWrapper);
        var dataTableCreator = new DataTableCreator(sinkOptions.TableName, columnOptions);
        var sqlCreateTableWriter = new SqlCreateTableWriter(sinkOptions.TableName, columnOptions, dataTableCreator);

        var sqlConnectionStringBuilderWrapperNoDb = new OracleConnectionStringBuilderWrapper(
            connectionString, sinkOptions.EnlistInTransaction)
        {
            DataSource = ""
        };
        var sqlConnectionFactoryNoDb =
            new SqlConnectionFactory(sqlConnectionStringBuilderWrapperNoDb);

        var logEventDataGenerator =
            new LogEventDataGenerator(columnOptions,
                new StandardColumnDataGenerator(columnOptions, formatProvider,
                    new XmlPropertyFormatter(),
                    logEventFormatter),
                new AdditionalColumnDataGenerator(
                    new ColumnSimplePropertyValueResolver(),
                    new ColumnHierarchicalPropertyValueResolver()));

        var sinkDependencies = new SinkDependencies
        {
            DataTableCreator = dataTableCreator,
            SqlTableCreator = new SqlTableCreator(
                sqlCreateTableWriter, sqlConnectionFactory),
            SqlBulkBatchWriter = sinkOptions.UseSqlBulkCopy
                ? (ISqlBulkBatchWriter)new SqlBulkBatchWriter(
                    sinkOptions.TableName, columnOptions.DisableTriggers,
                    sqlConnectionFactory, logEventDataGenerator)
                : (ISqlBulkBatchWriter)new SqlInsertStatementWriter(
                    sinkOptions.TableName, sqlConnectionFactory, logEventDataGenerator),
            SqlLogEventWriter = new SqlInsertStatementWriter(
                sinkOptions.TableName,sqlConnectionFactory, logEventDataGenerator)
        };

        return sinkDependencies;
    }
}

using System.Data;
using Serilog.Formatting;
using Serilog.Sinks.Oracle.Dependencies;
using Serilog.Sinks.Oracle.Platform;

namespace Serilog.Sinks.Oracle;
public class OracleSink : IBatchedLogEventSink, IDisposable
{
    private readonly ISqlBulkBatchWriter _sqlBulkBatchWriter;
    private readonly DataTable _eventTable;

    /// <summary>
    /// The default database schema name.
    /// </summary>
    public const string DefaultSchemaName = "dbo";

    /// <summary>
    /// A reasonable default for the number of events posted in each batch.
    /// </summary>
    public const int DefaultBatchPostingLimit = 50;

    /// <summary>
    /// A reasonable default time to wait between checking for event batches.
    /// </summary>
    public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(5);

    private bool _disposedValue;

    public OracleSink(
           string connectionString,
           OracleSinkOptions sinkOptions,
           IFormatProvider formatProvider = null,
           ColumnOptions columnOptions = null,
           ITextFormatter logEventFormatter = null)
           : this(sinkOptions, SinkDependenciesFactory.Create(connectionString, sinkOptions, formatProvider, columnOptions, logEventFormatter))
    {
    }

    // Internal constructor with injectable dependencies for better testability
    internal OracleSink(
        OracleSinkOptions sinkOptions,
        SinkDependencies sinkDependencies)
    {
        ValidateParameters(sinkOptions);
        CheckSinkDependencies(sinkDependencies);

        _sqlBulkBatchWriter = sinkDependencies.SqlBulkBatchWriter;
        _eventTable = sinkDependencies.DataTableCreator.CreateDataTable();

        CreateDatabaseAndTable(sinkOptions, sinkDependencies);
    }

    /// <summary>
    /// Emit a batch of log events, running asynchronously.
    /// </summary>
    /// <param name="batch">The events to emit.</param>
    public Task EmitBatchAsync(IReadOnlyCollection<LogEvent> batch) =>
        _sqlBulkBatchWriter.WriteBatch(batch, _eventTable);

    /// <summary>
    /// Called upon batchperiod if no data is in batch. Not used by this sink.
    /// </summary>
    /// <returns>A completed task</returns>
    public Task OnEmptyBatchAsync() =>
        Task.CompletedTask;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the Serilog.Sinks.MSSqlServer.MSSqlServerAuditSink and optionally
    /// releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _eventTable.Dispose();
            }

            _disposedValue = true;
        }
    }

    private static void ValidateParameters(OracleSinkOptions sinkOptions)
    {
        if (sinkOptions?.TableName == null)
        {
            throw new InvalidOperationException("Table name must be specified!");
        }
    }

    private static void CheckSinkDependencies(SinkDependencies sinkDependencies)
    {
        if (sinkDependencies == null)
        {
            throw new ArgumentNullException(nameof(sinkDependencies));
        }

        if (sinkDependencies.DataTableCreator == null)
        {
            throw new InvalidOperationException("DataTableCreator is not initialized!");
        }

        if (sinkDependencies.SqlTableCreator == null)
        {
            throw new InvalidOperationException("SqlTableCreator is not initialized!");
        }

        if (sinkDependencies.SqlBulkBatchWriter == null)
        {
            throw new InvalidOperationException("SqlBulkBatchWriter is not initialized!");
        }
    }

    private void CreateDatabaseAndTable(OracleSinkOptions sinkOptions, SinkDependencies sinkDependencies)
    {
        if (sinkOptions.AutoCreateSqlDatabase)
        {
            sinkDependencies.SqlDatabaseCreator.Execute();
        }

        if (sinkOptions.AutoCreateSqlTable)
        {
            sinkDependencies.SqlTableCreator.Execute();
        }
    }
}

using Serilog.Formatting;
using Serilog.Sinks.Oracle.Dependencies;
using Serilog.Sinks.Oracle.Platform;
using static System.FormattableString;

namespace Serilog.Sinks.Oracle;
public class OracleAuditSink : ILogEventSink, IDisposable
{
    private readonly ISqlLogEventWriter _sqlLogEventWriter;


    /// <summary>
    /// Construct a sink posting to the specified database.
    /// </summary>
    /// <param name="connectionString">Connection string to access the database.</param>
    /// <param name="sinkOptions">Supplies additional options for the sink</param>
    /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
    /// <param name="columnOptions">Options that pertain to columns</param>
    /// <param name="logEventFormatter">Supplies custom formatter for the LogEvent column, or null</param>
    public OracleAuditSink(
          string connectionString,
          OracleSinkOptions sinkOptions,
          IFormatProvider formatProvider = null,
          ColumnOptions columnOptions = null,
          ITextFormatter logEventFormatter = null)
          : this(sinkOptions, columnOptions,
                SinkDependenciesFactory.Create(connectionString, sinkOptions, formatProvider, columnOptions, logEventFormatter))
    {
    }

    // Internal constructor with injectable dependencies for better testability
    internal OracleAuditSink(
        OracleSinkOptions sinkOptions,
        ColumnOptions columnOptions,
        SinkDependencies sinkDependencies)
    {
        ValidateParameters(sinkOptions, columnOptions);
        CheckSinkDependencies(sinkDependencies);

        _sqlLogEventWriter = sinkDependencies.SqlLogEventWriter;

        CreateDatabaseAndTable(sinkOptions, sinkDependencies);
    }

    /// <summary>Emit the provided log event to the sink.</summary>
    /// <param name="logEvent">The log event to write.</param>
    public void Emit(LogEvent logEvent) =>
        _sqlLogEventWriter.WriteEvent(logEvent);

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the Serilog.Sinks.MSSqlServer.MSSqlServerAuditSink and optionally
    /// releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        // This class needn't to dispose anything. This is just here for sink interface compatibility.
    }

    private static void ValidateParameters(OracleSinkOptions sinkOptions, ColumnOptions columnOptions)
    {
        if (sinkOptions?.TableName == null)
        {
            throw new InvalidOperationException("Table name must be specified!");
        }

        if (columnOptions.DisableTriggers)
            throw new NotSupportedException(Invariant($"The {nameof(ColumnOptions.DisableTriggers)} option is not supported for auditing."));
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

        if (sinkDependencies.SqlLogEventWriter == null)
        {
            throw new InvalidOperationException("SqlLogEventWriter is not initialized!");
        }
    }

    private static void CreateDatabaseAndTable(OracleSinkOptions sinkOptions, SinkDependencies sinkDependencies)
    {

        if (sinkOptions.AutoCreateSqlTable)
        {
            sinkDependencies.SqlTableCreator.Execute();
        }
    }

}

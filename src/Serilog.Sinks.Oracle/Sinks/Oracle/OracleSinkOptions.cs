namespace Serilog.Sinks.Oracle;
public class OracleSinkOptions
{
    /// <summary>
    /// Initializes a new <see cref="OracleServerSinkOptions"/> instance with default values.
    /// </summary>
    public OracleSinkOptions()
    {
        BatchPostingLimit = OracleSink.DefaultBatchPostingLimit;
        BatchPeriod = OracleSink.DefaultPeriod;
        EagerlyEmitFirstEvent = true;
        UseSqlBulkCopy = true;
    }

    internal OracleSinkOptions(
        string tableName,
        int? batchPostingLimit,
        TimeSpan? batchPeriod,
        bool autoCreateSqlTable,
        string schemaName) : this()
    {
        TableName = tableName;
        BatchPostingLimit = batchPostingLimit ?? BatchPostingLimit;
        BatchPeriod = batchPeriod ?? BatchPeriod;
        AutoCreateSqlTable = autoCreateSqlTable;
    }

    /// <summary>
    /// Name of the database table for writing the log events
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Flag to automatically create the log events database if it does not exist (default: false)
    /// </summary>
    public bool AutoCreateSqlDatabase { get; set; }

    /// <summary>
    /// Flag to automatically create the log events table if it does not exist (default: false)
    /// </summary>
    public bool AutoCreateSqlTable { get; set; }

    /// <summary>
    /// Flag to make logging SQL commands take part in ambient transactions (default: false)
    /// </summary>
    public bool EnlistInTransaction { get; set; }

    /// <summary>
    /// Limits how many log events are written to the database per batch (default: 50)
    /// </summary>
    public int BatchPostingLimit { get; set; }

    /// <summary>
    /// Time span until a batch of log events is written to the database (default: 5 seconds)
    /// </summary>
    public TimeSpan BatchPeriod { get; set; }

    /// <summary>
    /// Flag to eagerly emit a batch containing the first received event (default: true)
    /// </summary>
    public bool EagerlyEmitFirstEvent { get; set; }

    /// <summary>
    /// A switch allowing the pass-through minimum level to be changed at runtime
    /// </summary>
    public LoggingLevelSwitch LevelSwitch { get; set; }

    /// <summary>
    /// Flag to use <see cref="Microsoft.Data.SqlClient.SqlBulkCopy"/> instead of individual INSERT statements (default: true)
    /// </summary>
    public bool UseSqlBulkCopy { get; set; }
}

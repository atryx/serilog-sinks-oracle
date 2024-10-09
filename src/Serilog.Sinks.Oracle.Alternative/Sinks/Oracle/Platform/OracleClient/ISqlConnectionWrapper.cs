namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal interface ISqlConnectionWrapper : IDisposable
{
    string ConnectionString { get; }

    void Open();
    Task OpenAsync();
    ISqlCommandWrapper CreateCommand();
    ISqlCommandWrapper CreateCommand(string cmdText);
    ISqlBulkCopyWrapper CreateSqlBulkCopy(string destinationTableName);
}

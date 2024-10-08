using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal class SqlConnectionWrapper : ISqlConnectionWrapper
{
    private readonly OracleConnection _sqlConnection;
    private bool _disposedValue;

    public SqlConnectionWrapper(string connectionString)
    {
        _sqlConnection = new OracleConnection(connectionString);
    }

    public string ConnectionString => _sqlConnection.ConnectionString;

    public void Open()
    {
        _sqlConnection.Open();
    }

    public async Task OpenAsync()
    {
        await _sqlConnection.OpenAsync().ConfigureAwait(false);
    }

    public ISqlCommandWrapper CreateCommand()
    {
        var sqlCommand = _sqlConnection.CreateCommand();
        return new SqlCommandWrapper(sqlCommand);
    }

    public ISqlCommandWrapper CreateCommand(string cmdText)
    {
        var sqlCommand = new OracleCommand(cmdText, _sqlConnection);
        return new SqlCommandWrapper(sqlCommand);
    }

    public ISqlBulkCopyWrapper CreateSqlBulkCopy(string destinationTableName)
    {
        var sqlBulkCopy = new OracleBulkCopy(_sqlConnection);
        sqlBulkCopy.DestinationTableName = destinationTableName;

        return new SqlBulkCopyWrapper(sqlBulkCopy);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _sqlConnection.Dispose();
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

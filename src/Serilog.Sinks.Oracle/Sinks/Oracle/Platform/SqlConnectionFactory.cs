using Serilog.Sinks.Oracle.Platform.SqlClient;

namespace Serilog.Sinks.Oracle.Platform;
internal class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;
    private readonly IOracleConnectionStringBuilderWrapper _sqlConnectionStringBuilderWrapper;

    public SqlConnectionFactory(IOracleConnectionStringBuilderWrapper sqlConnectionStringBuilderWrapper)
    {
        _sqlConnectionStringBuilderWrapper = sqlConnectionStringBuilderWrapper
            ?? throw new ArgumentNullException(nameof(sqlConnectionStringBuilderWrapper));

        _connectionString = _sqlConnectionStringBuilderWrapper.ConnectionString;
    }

    public ISqlConnectionWrapper Create()
    {
        return new SqlConnectionWrapper(_connectionString);
    }
}

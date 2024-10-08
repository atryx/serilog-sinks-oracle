namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal interface IOracleConnectionStringBuilderWrapper
{
    string ConnectionString { get; }
    string DataSource { get; }
}

using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal class OracleConnectionStringBuilderWrapper : IOracleConnectionStringBuilderWrapper
{
    private readonly OracleConnectionStringBuilder _sqlConnectionStringBuilder;

    public OracleConnectionStringBuilderWrapper(string connectionString, bool enlist)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        _sqlConnectionStringBuilder = new OracleConnectionStringBuilder(connectionString)
        {
            Enlist = enlist.ToString().ToLower()
        };
    }

    public string ConnectionString => _sqlConnectionStringBuilder.ConnectionString;

    public string DataSource
    {
        get => _sqlConnectionStringBuilder.DataSource;
        set => _sqlConnectionStringBuilder.DataSource = value;
    }
}

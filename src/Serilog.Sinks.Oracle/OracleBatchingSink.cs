using Oracle.ManagedDataAccess.Client;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace Serilog.Sinks.Oracle;

public class OracleBatchingSink : PeriodicBatchingSink
{
    private readonly string _connectionString;

    public OracleBatchingSink(string connectionString, int batchSizeLimit, TimeSpan period)
        : base(batchSizeLimit, period)
    {
        _connectionString = connectionString;
    }

    protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
        using (var connection = new OracleConnection(_connectionString))
        {
            connection.Open();

            foreach (var logEvent in events)
            {
                var message = logEvent.RenderMessage();
                var logLevel = logEvent.Level.ToString();
                var timestamp = logEvent.Timestamp;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO LogTable (Timestamp, LogLevel, Message) 
                                            VALUES (:Timestamp, :LogLevel, :Message)";

                    command.Parameters.Add(":Timestamp", OracleDbType.Date).Value = timestamp;
                    command.Parameters.Add(":LogLevel", OracleDbType.Varchar2).Value = logLevel;
                    command.Parameters.Add(":Message", OracleDbType.Varchar2).Value = message;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

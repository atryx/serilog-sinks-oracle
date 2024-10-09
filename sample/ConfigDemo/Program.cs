using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration.Extensions;
using Serilog.Sinks.Oracle;

public static class Program
{
    private const string _connectionStringName = "LogDatabase";
    private const string _tableName = "Logs7";

    public static void Main()
    {
        try
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var columnOptionsSection = configuration.GetSection("Serilog:ColumnOptions");
            var sinkOptionsSection = configuration.GetSection("Serilog:SinkOptions");

            // New SinkOptions based interface
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Oracle(
                    connectionString: _connectionStringName,
                    sinkOptions: new OracleSinkOptions
                    {
                        TableName = _tableName,
                        AutoCreateSqlTable = true
                    },
                    sinkOptionsSection: sinkOptionsSection,
                    appConfiguration: configuration,
                    columnOptionsSection: columnOptionsSection)
                .CreateLogger();


            Log.Information("Hello {Name} from thread {ThreadId}", Environment.GetEnvironmentVariable("USERNAME"), Environment.CurrentManagedThreadId);

            Log.Warning("No coins remain at position {@Position}", new { Lat = 25, Long = 134 });

            Log.CloseAndFlush();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
        }

    }
}

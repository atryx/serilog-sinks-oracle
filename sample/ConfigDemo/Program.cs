using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration.Extensions;
using Serilog.Sinks.Oracle;

public static class Program
{
    private const string _connectionStringName = "LogDatabase";
    private const string _schemaName = "";
    private const string _tableName = "Logs";

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
                    SchemaName = _schemaName,
                    AutoCreateSqlTable = true
                },
                sinkOptionsSection: sinkOptionsSection,
                appConfiguration: configuration,
                columnOptionsSection: columnOptionsSection)
            .CreateLogger();

        Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));


        Log.Information("Hello {Name} from thread {ThreadId}", Environment.GetEnvironmentVariable("USERNAME"), Environment.CurrentManagedThreadId);

            Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));

            Log.Warning("No coins remain at position {@Position}", new { Lat = 25, Long = 134 });

            Serilog.Debugging.SelfLog.Enable(msg => Console.Error.WriteLine(msg));

            Log.CloseAndFlush();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error : {ex.Message}");
        }

    }
}

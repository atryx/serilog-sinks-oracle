namespace Serilog.Sinks.Oracle.Output;

internal interface ILogEventDataGenerator
{
    IEnumerable<KeyValuePair<string, object>> GetColumnsAndValues(LogEvent logEvent);
}

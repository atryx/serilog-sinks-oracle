namespace Serilog.Sinks.Oracle.Output;

internal interface IStandardColumnDataGenerator
{
    KeyValuePair<string, object> GetStandardColumnNameAndValue(StandardColumn column, LogEvent logEvent);
}

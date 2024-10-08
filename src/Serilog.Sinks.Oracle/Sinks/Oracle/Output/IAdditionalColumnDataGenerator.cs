namespace Serilog.Sinks.Oracle.Output;

internal interface IAdditionalColumnDataGenerator
{
    KeyValuePair<string, object> GetAdditionalColumnNameAndValue(OracleColumn additionalColumn, IReadOnlyDictionary<string, LogEventPropertyValue> properties);
}

namespace Serilog.Sinks.Oracle.Output;

internal interface IColumnSimplePropertyValueResolver
{
    KeyValuePair<string, LogEventPropertyValue> GetPropertyValueForColumn(OracleColumn additionalColumn, IReadOnlyDictionary<string, LogEventPropertyValue> properties);
}

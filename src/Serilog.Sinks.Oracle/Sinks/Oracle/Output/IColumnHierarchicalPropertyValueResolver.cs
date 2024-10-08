namespace Serilog.Sinks.Oracle.Output;

internal interface IColumnHierarchicalPropertyValueResolver
{
    KeyValuePair<string, LogEventPropertyValue> GetPropertyValueForColumn(OracleColumn additionalColumn, IReadOnlyDictionary<string, LogEventPropertyValue> properties);
}

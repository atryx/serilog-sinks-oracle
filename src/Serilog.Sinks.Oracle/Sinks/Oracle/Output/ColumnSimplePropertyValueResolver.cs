namespace Serilog.Sinks.Oracle.Output;

internal class ColumnSimplePropertyValueResolver : IColumnSimplePropertyValueResolver
{
    public KeyValuePair<string, LogEventPropertyValue> GetPropertyValueForColumn(OracleColumn additionalColumn, IReadOnlyDictionary<string, LogEventPropertyValue> properties)
    {
        return properties.FirstOrDefault(p => p.Key == additionalColumn.PropertyName);
    }
}

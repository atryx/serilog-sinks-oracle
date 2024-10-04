namespace Serilog.Sinks.Oracle.Configuration;

internal interface IMicrosoftExtensionsColumnOptionsProvider
{
    ColumnOptions ConfigureColumnOptions(ColumnOptions columnOptions, IConfigurationSection config);
}

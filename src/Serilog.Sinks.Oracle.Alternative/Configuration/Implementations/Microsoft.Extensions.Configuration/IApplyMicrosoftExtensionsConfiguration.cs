namespace Serilog.Sinks.Oracle;

internal interface IApplyMicrosoftExtensionsConfiguration
{
    string GetConnectionString(string nameOrConnectionString, IConfiguration appConfiguration);
    ColumnOptions ConfigureColumnOptions(ColumnOptions columnOptions, IConfigurationSection config);
    OracleSinkOptions ConfigureSinkOptions(OracleSinkOptions sinkOptions, IConfigurationSection config);
}

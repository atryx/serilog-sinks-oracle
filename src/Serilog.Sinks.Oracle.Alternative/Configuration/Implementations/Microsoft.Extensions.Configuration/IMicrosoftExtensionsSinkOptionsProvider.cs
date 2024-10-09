namespace Serilog.Sinks.Oracle.Configuration;

internal interface IMicrosoftExtensionsSinkOptionsProvider
{
    OracleSinkOptions ConfigureSinkOptions(OracleSinkOptions sinkOptions, IConfigurationSection config);
}

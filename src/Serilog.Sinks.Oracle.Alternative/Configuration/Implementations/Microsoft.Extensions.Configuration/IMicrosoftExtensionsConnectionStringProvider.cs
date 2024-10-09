namespace Serilog.Sinks.Oracle.Configuration;

internal interface IMicrosoftExtensionsConnectionStringProvider
{
    string GetConnectionString(string nameOrConnectionString, IConfiguration appConfiguration);
}

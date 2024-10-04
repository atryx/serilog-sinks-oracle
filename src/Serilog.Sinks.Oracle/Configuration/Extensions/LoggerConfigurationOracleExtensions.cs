using Serilog.Formatting;
using Serilog.Sinks.MSSqlServer.Configuration.Factories;
using Serilog.Sinks.Oracle;

namespace Serilog.Configuration.Extensions;

public static class LoggerConfigurationPostgreSqlExtensions
{
    public static LoggerConfiguration Oracle(
        this LoggerSinkConfiguration loggerConfiguration,
        string connectionString,
        OracleSinkOptions sinkOptions = null,
        IConfigurationSection sinkOptionSection = null,
        IConfiguration appConfiguration = null,
        LogEventLevel restictredToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider formatProvider = null,
        ColumnOptions columnOptions = null,
        IConfigurationSection columnOptionsSection = null,
        ITextFormatter logEventFormatter = null)
    {
        if (loggerConfiguration == null)
            throw new ArgumentNullException(nameof(loggerConfiguration));

        ReadConfiguration(ref connectionString, ref sinkOptions, appConfiguration, ref columnOptions, columnOptionsSection, sinkOptionSection);

        IOracleSinkFactory sinkFactory = new OracleSinkFactory();
        var sink = sinkFactory.Create(connectionString, sinkOptions, formatProvider, columnOptions, logEventFormatter);

        IPeriodicBatchingSinkFactory periodicBatchingSinkFactory = new PeriodicBatchingSinkFactory();
        var periodicBatchingSink = periodicBatchingSinkFactory.Create(sink, sinkOptions);

        return loggerConfiguration.Sink(periodicBatchingSink, restictredToMinimumLevel, sinkOptions?.LevelSwitch);
    }

    public static LoggerConfiguration Oracle(
        this LoggerAuditSinkConfiguration loggerAuditSinkConfiguration,
        string connectionString,
        OracleSinkOptions sinkOptions = null,
        IConfigurationSection sinkOptionSection = null,
        IConfiguration appConfiguration = null,
        LogEventLevel restictredToMinimumLevel = LevelAlias.Minimum,
        IFormatProvider formatProvider = null,
        ColumnOptions columnOptions = null,
        IConfigurationSection columnOptionsSection = null,
        ITextFormatter logEventFormatter = null)
    {
        if (loggerAuditSinkConfiguration == null)
            throw new ArgumentNullException(nameof(loggerAuditSinkConfiguration));

        ReadConfiguration(ref connectionString, ref sinkOptions, appConfiguration, ref columnOptions, columnOptionsSection, sinkOptionSection);

        IOracleAuditSinkFactory auditSinkFactory = new OracleAuditSinkFactory();
        var auditSink = auditSinkFactory.Create(connectionString, sinkOptions, formatProvider, columnOptions, logEventFormatter);

        return loggerAuditSinkConfiguration.Sink(auditSink, restictredToMinimumLevel, sinkOptions?.LevelSwitch);
    }

    private static void ReadConfiguration(
            ref string connectionString,
            ref OracleSinkOptions sinkOptions,
            IConfiguration appConfiguration,
            ref ColumnOptions columnOptions,
            IConfigurationSection columnOptionsSection,
            IConfigurationSection sinkOptionsSection)
    {
        sinkOptions = sinkOptions ?? new OracleSinkOptions();
        columnOptions = columnOptions ?? new ColumnOptions();

        IApplyMicrosoftExtensionsConfiguration microsoftExtensionsConfiguration = new ApplyMicrosoftExtensionsConfiguration();
        connectionString = microsoftExtensionsConfiguration.GetConnectionString(connectionString, appConfiguration);
        columnOptions = microsoftExtensionsConfiguration.ConfigureColumnOptions(columnOptions, columnOptionsSection);
        sinkOptions = microsoftExtensionsConfiguration.ConfigureSinkOptions(sinkOptions, sinkOptionsSection);
    }
}

using System.Globalization;

namespace Serilog.Sinks.Oracle.Configuration;

internal class MicrosoftExtensionsSinkOptionsProvider : IMicrosoftExtensionsSinkOptionsProvider
{
    public OracleSinkOptions ConfigureSinkOptions(OracleSinkOptions sinkOptions, IConfigurationSection config)
    {
        if (config == null)
        {
            return sinkOptions;
        }

        ReadTableOptions(config, sinkOptions);
        ReadBatchSettings(config, sinkOptions);

        return sinkOptions;
    }

    private static void ReadTableOptions(IConfigurationSection config, OracleSinkOptions sinkOptions)
    {
        SetProperty.IfNotNull<string>(config["tableName"], val => sinkOptions.TableName = val);
        SetProperty.IfNotNull<bool>(config["autoCreateSqlDatabase"], val => sinkOptions.AutoCreateSqlDatabase = val);
        SetProperty.IfNotNull<bool>(config["autoCreateSqlTable"], val => sinkOptions.AutoCreateSqlTable = val);
        SetProperty.IfNotNull<bool>(config["enlistInTransaction"], val => sinkOptions.EnlistInTransaction = val);
    }

    private static void ReadBatchSettings(IConfigurationSection config, OracleSinkOptions sinkOptions)
    {
        SetProperty.IfNotNull<int>(config["batchPostingLimit"], val => sinkOptions.BatchPostingLimit = val);
        SetProperty.IfNotNull<string>(config["batchPeriod"], val => sinkOptions.BatchPeriod = TimeSpan.Parse(val, CultureInfo.InvariantCulture));
        SetProperty.IfNotNull<bool>(config["eagerlyEmitFirstEvent"], val => sinkOptions.EagerlyEmitFirstEvent = val);
        SetProperty.IfNotNull<bool>(config["useSqlBulkCopy"], val => sinkOptions.UseSqlBulkCopy = val);
    }
}

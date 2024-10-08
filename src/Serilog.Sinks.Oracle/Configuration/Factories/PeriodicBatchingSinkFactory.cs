namespace Serilog.Sinks.Oracle.Configuration.Factories;

internal class PeriodicBatchingSinkFactory : IPeriodicBatchingSinkFactory
{
    public ILogEventSink Create(IBatchedLogEventSink sink, OracleSinkOptions sinkOptions)
    {
        var periodicBatchingSinkOptions = new BatchingOptions
        {
            BatchSizeLimit = sinkOptions.BatchPostingLimit,
            BufferingTimeLimit = sinkOptions.BatchPeriod,
            EagerlyEmitFirstEvent = sinkOptions.EagerlyEmitFirstEvent
        };
        return LoggerSinkConfiguration.CreateSink(lc => lc.Sink(sink, periodicBatchingSinkOptions));
    }
}

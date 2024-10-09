namespace Serilog.Sinks.Oracle.Configuration.Factories;

internal interface IPeriodicBatchingSinkFactory
{
    ILogEventSink Create(IBatchedLogEventSink sink, OracleSinkOptions sinkOptions);
}

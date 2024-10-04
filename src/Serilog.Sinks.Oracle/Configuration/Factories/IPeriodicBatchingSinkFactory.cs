using Serilog.Sinks.Oracle;

namespace Serilog.Sinks.MSSqlServer.Configuration.Factories;

internal interface IPeriodicBatchingSinkFactory
{
    ILogEventSink Create(IBatchedLogEventSink sink, OracleSinkOptions sinkOptions);
}

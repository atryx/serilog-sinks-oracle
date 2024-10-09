namespace Serilog.Sinks.Oracle.Platform;

internal interface ISqlLogEventWriter
{
    void WriteEvent(LogEvent logEvent);
}

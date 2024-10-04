using Serilog.Formatting;
using Serilog.Sinks.Oracle;

namespace Serilog.Sinks.MSSqlServer.Configuration.Factories;

internal interface IOracleAuditSinkFactory
{
    ILogEventSink Create(
        string connectionString,
        OracleSinkOptions sinkOptions,
        IFormatProvider formatProvider,
        ColumnOptions columnOptions,
        ITextFormatter logEventFormatter);
}

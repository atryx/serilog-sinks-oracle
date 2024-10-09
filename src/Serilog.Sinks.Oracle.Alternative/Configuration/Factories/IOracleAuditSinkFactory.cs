using Serilog.Formatting;

namespace Serilog.Sinks.Oracle.Configuration.Factories;

internal interface IOracleAuditSinkFactory
{
    ILogEventSink Create(
        string connectionString,
        OracleSinkOptions sinkOptions,
        IFormatProvider formatProvider,
        ColumnOptions columnOptions,
        ITextFormatter logEventFormatter);
}

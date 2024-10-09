using Serilog.Formatting;

namespace Serilog.Sinks.Oracle.Configuration.Factories;

internal class OracleAuditSinkFactory : IOracleAuditSinkFactory
{
    public ILogEventSink Create(
        string connectionString,
        OracleSinkOptions sinkOptions,
        IFormatProvider formatProvider,
        ColumnOptions columnOptions,
        ITextFormatter logEventFormatter) =>
        new OracleAuditSink(
            connectionString,
            sinkOptions,
            formatProvider,
            columnOptions,
            logEventFormatter);
}

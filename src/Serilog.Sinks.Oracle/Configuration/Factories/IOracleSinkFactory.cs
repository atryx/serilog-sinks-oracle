using Serilog.Formatting;
using Serilog.Sinks.Oracle;

namespace Serilog.Sinks.MSSqlServer.Configuration.Factories;

internal interface IOracleSinkFactory
{
    IBatchedLogEventSink Create(
        string connectionString,
        OracleSinkOptions sinkOptions,
        IFormatProvider formatProvider,
        ColumnOptions columnOptions,
        ITextFormatter logEventFormatter);
}

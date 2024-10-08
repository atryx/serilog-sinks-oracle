using Serilog.Formatting;

namespace Serilog.Sinks.Oracle.Configuration.Factories
{
    internal class OracleSinkFactory : IOracleSinkFactory
    {
        public IBatchedLogEventSink Create(
            string connectionString,
            OracleSinkOptions sinkOptions,
            IFormatProvider formatProvider,
            ColumnOptions columnOptions,
            ITextFormatter logEventFormatter) =>
            new OracleSink(
                connectionString,
                sinkOptions,
                formatProvider,
                columnOptions,
                logEventFormatter);
    }
}

using System.Data;

namespace Serilog.Sinks.Oracle.Platform;

internal interface ISqlBulkBatchWriter
{
    Task WriteBatch(IEnumerable<LogEvent> events, DataTable dataTable);
}

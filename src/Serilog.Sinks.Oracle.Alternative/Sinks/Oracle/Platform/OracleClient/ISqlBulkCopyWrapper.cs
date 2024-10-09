using System.Data;

namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal interface ISqlBulkCopyWrapper : IDisposable
{
    void AddSqlBulkCopyColumnMapping(string sourceColumn, string destinationColumn);
    void WriteToServer(DataTable table);
}

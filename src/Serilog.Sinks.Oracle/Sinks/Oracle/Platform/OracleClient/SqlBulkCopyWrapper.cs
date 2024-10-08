using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal class SqlBulkCopyWrapper : ISqlBulkCopyWrapper
{
    private readonly OracleBulkCopy _oracleBulkCopy;
    private bool _disposedValue;

    public SqlBulkCopyWrapper(OracleBulkCopy sqlBulkCopy)
    {
        _oracleBulkCopy = sqlBulkCopy ?? throw new ArgumentNullException(nameof(sqlBulkCopy));
    }

    public void AddSqlBulkCopyColumnMapping(string sourceColumn, string destinationColumn)
    {
        var mapping = new OracleBulkCopyColumnMapping(sourceColumn, destinationColumn);
        _oracleBulkCopy.ColumnMappings.Add(mapping);
    }

    public void WriteToServer(DataTable table) =>
        _oracleBulkCopy.WriteToServer(table);

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            ((IDisposable)_oracleBulkCopy).Dispose();
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

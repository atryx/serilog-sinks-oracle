using System.Data;

namespace Serilog.Sinks.Oracle.Platform;

internal interface IDataTableCreator
{
    DataTable CreateDataTable();
}

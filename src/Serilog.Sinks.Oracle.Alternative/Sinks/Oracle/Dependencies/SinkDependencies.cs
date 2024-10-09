using Serilog.Sinks.Oracle.Platform;

namespace Serilog.Sinks.Oracle.Dependencies;
internal class SinkDependencies
{
    public IDataTableCreator DataTableCreator { get; set; }
    public ISqlCommandExecutor SqlTableCreator { get; set; }
    public ISqlBulkBatchWriter SqlBulkBatchWriter { get; set; }
    public ISqlLogEventWriter SqlLogEventWriter { get; set; }
}

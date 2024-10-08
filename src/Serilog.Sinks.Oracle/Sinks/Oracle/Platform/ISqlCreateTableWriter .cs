namespace Serilog.Sinks.Oracle.Platform;
internal interface ISqlCreateTableWriter : ISqlWriter
{
    string TableName { get; }
}

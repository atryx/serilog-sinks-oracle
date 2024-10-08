namespace Serilog.Sinks.Oracle.Platform;
internal interface ISqlCreateDatabaseWriter : ISqlWriter
{
    string DatabaseName { get; }
}

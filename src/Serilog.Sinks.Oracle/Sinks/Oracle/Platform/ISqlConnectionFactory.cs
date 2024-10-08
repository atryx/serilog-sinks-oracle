using Serilog.Sinks.Oracle.Platform.SqlClient;

namespace Serilog.Sinks.Oracle.Platform;
internal interface ISqlConnectionFactory
{
    ISqlConnectionWrapper Create();
}

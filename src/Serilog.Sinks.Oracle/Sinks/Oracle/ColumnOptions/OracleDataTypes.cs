using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle;

/// <summary>
/// 
/// </summary>
public static class OracleDataTypes
{
    /// <summary>
    /// A collection keyed on the OracleDbType enum with values representing the equivalent DataColumn .NET type.
    /// </summary>
    public static readonly Dictionary<OracleDbType, Type> SystemTypeMap = new Dictionary<OracleDbType, Type>
        {
            // mapping reference
            // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings

            { OracleDbType.Int64, typeof(long) },
            { OracleDbType.Boolean, typeof(bool) },
            { OracleDbType.Char, typeof(string) },
            { OracleDbType.Date, typeof(DateTime) },
            { OracleDbType.TimeStamp, typeof(DateTime) },
            { OracleDbType.TimeStampTZ, typeof(DateTimeOffset) },
            { OracleDbType.Decimal, typeof(decimal) },
            { OracleDbType.Double, typeof(double) },
            { OracleDbType.Int32, typeof(int) },
            { OracleDbType.NChar, typeof(string) },
            { OracleDbType.NVarchar2, typeof(string) },
            { OracleDbType.Single, typeof(float) },
            { OracleDbType.Int16, typeof(short) },
            { OracleDbType.IntervalDS, typeof(TimeSpan) },
            { OracleDbType.Byte, typeof(byte) },
            { OracleDbType.Raw, typeof(Guid) },
            { OracleDbType.Varchar2, typeof(string) },
            { OracleDbType.XmlType, typeof(string) }
        };

    /// <summary>
    /// SQL column types for supported strings
    /// </summary>
    public static readonly ReadOnlyCollection<OracleDbType> VariableCharacterColumnTypes = new ReadOnlyCollection<OracleDbType>(new List<OracleDbType> {
            OracleDbType.NVarchar2,
            OracleDbType.Varchar2
        });

    /// <summary>
    /// The SQL column types which require a non-zero DataLength property.
    /// </summary>
    public static readonly List<OracleDbType> DataLengthRequired = new List<OracleDbType>
        {
            OracleDbType.Char,
            OracleDbType.NChar,
            OracleDbType.NVarchar2,
            OracleDbType.Varchar2
        };

    /// <summary>
    /// Like Enum.TryParse for OracleDbType but it also validates against the SqlTypeToSystemType list, returning
    /// false if the requested SQL type is not supported by this sink.
    /// </summary>
    public static bool TryParseIfSupported(string requestedType, out OracleDbType supportedOracleDbType)
    {
        if (Enum.TryParse(requestedType, ignoreCase: true, result: out supportedOracleDbType))
        {
            return SystemTypeMap.ContainsKey(supportedOracleDbType);
        }
        return false;
    }

    /// <summary>
    /// A collection keyed on the DataColumn .NET types with values representing the default OracleDbType enum.
    /// This exists for backwards-compatibility reasons since all configuration based on DataColumn has been
    /// marked Obsolete and will be removed in a future release.
    /// </summary>
    public static readonly Dictionary<Type, OracleDbType> ReverseTypeMap = new Dictionary<Type, OracleDbType>
        {
            { typeof(long), OracleDbType.Int64 },
            { typeof(bool), OracleDbType.Boolean },
            { typeof(DateTime), OracleDbType.TimeStamp },
            { typeof(DateTimeOffset), OracleDbType.TimeStampTZ},
            { typeof(decimal), OracleDbType.Decimal },
            { typeof(double), OracleDbType.Double },
            { typeof(int), OracleDbType.Int32 },
            { typeof(string), OracleDbType.NVarchar2 },
            { typeof(float), OracleDbType.Single },
            { typeof(short), OracleDbType.Int16 },
            { typeof(TimeSpan), OracleDbType.IntervalDS },
            { typeof(byte), OracleDbType.Byte },
            { typeof(Guid), OracleDbType.Raw },
            { typeof(char), OracleDbType.Char },
        };

    /// <summary>
    /// Clustered Columnstore Indexes only support a subset of the available SQL column types.
    /// </summary>
    public static readonly List<OracleDbType> ColumnstoreCompatible = new List<OracleDbType>
    {
        OracleDbType.TimeStampTZ,
        OracleDbType.TimeStamp,
        OracleDbType.Date,
        OracleDbType.IntervalDS,
        OracleDbType.Double,
        OracleDbType.Single,
        OracleDbType.Decimal,
        OracleDbType.Int64,
        OracleDbType.Int32,
        OracleDbType.Int16,
        OracleDbType.Byte,
        OracleDbType.Boolean,
        OracleDbType.NVarchar2,
        OracleDbType.NChar,
        OracleDbType.Varchar2,
        OracleDbType.Char,
        OracleDbType.Raw,
        OracleDbType.XmlType
    };


}

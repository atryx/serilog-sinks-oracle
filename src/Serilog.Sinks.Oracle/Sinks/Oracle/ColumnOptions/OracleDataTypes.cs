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
    public static readonly Dictionary<OracleDbType, string> SystemTypeMap = new Dictionary<OracleDbType, string>
        {
            // mapping reference

            { OracleDbType.Int32, "NUMBER" },          // Int32 maps to NUMBER
            { OracleDbType.Int16, "NUMBER" },          // Int16 maps to NUMBER
            { OracleDbType.Int64, "NUMBER" },          // Int64 maps to NUMBER
            { OracleDbType.Byte, "NUMBER" },           // Byte maps to NUMBER
            { OracleDbType.Decimal, "NUMBER" },        // Decimal maps to NUMBER
            { OracleDbType.Double, "FLOAT" },          // Double maps to FLOAT
            { OracleDbType.Single, "FLOAT" },          // Single maps to FLOAT
            { OracleDbType.Char, "CHAR" },             // Char maps to CHAR
            { OracleDbType.NChar, "NCHAR" },           // NChar maps to NCHAR
            { OracleDbType.Varchar2, "VARCHAR2" },     // Varchar2 maps to VARCHAR2
            { OracleDbType.NVarchar2, "NVARCHAR2" },   // NVarchar2 maps to NVARCHAR2
            { OracleDbType.Date, "DATE" },             // Date maps to DATE
            { OracleDbType.TimeStamp, "TIMESTAMP" },   // TimeStamp maps to TIMESTAMP
            { OracleDbType.TimeStampTZ, "TIMESTAMP WITH TIME ZONE" }, // TimeStampTZ maps to TIMESTAMP WITH TIME ZONE
            { OracleDbType.TimeStampLTZ, "TIMESTAMP WITH LOCAL TIME ZONE" }, // TimeStampLTZ maps to TIMESTAMP WITH LOCAL TIME ZONE
            { OracleDbType.Clob, "CLOB" },             // Clob maps to CLOB
            { OracleDbType.Blob, "BLOB" },             // Blob maps to BLOB
            { OracleDbType.XmlType, "XMLTYPE" },       // XmlType maps to XMLTYPE
            { OracleDbType.Raw, "RAW" },               // Raw maps to RAW
            { OracleDbType.BFile, "BFILE" },           // BFile maps to BFILE
            { OracleDbType.IntervalDS, "INTERVAL DAY TO SECOND" }, // IntervalDS maps to INTERVAL DAY TO SECOND
            { OracleDbType.IntervalYM, "INTERVAL YEAR TO MONTH" }, // IntervalYM maps to INTERVAL YEAR TO MONTH
            { OracleDbType.Boolean, "NUMBER(1)" },     // Boolean maps to NUMBER(1) (Oracle doesn't have native boolean)
            { OracleDbType.RefCursor, "REF CURSOR" }   // RefCursor maps to REF CURSOR
        };


    public static readonly Dictionary<OracleDbType, Type> OracleToCSharpTypeMap = new Dictionary<OracleDbType, Type>
    {
        { OracleDbType.Int16, typeof(short) },
        { OracleDbType.Int32, typeof(int) },
        { OracleDbType.Int64, typeof(long) },
        { OracleDbType.Boolean, typeof(bool) },
        { OracleDbType.Char, typeof(string) },
        { OracleDbType.NChar, typeof(string) },
        { OracleDbType.Varchar2, typeof(string) },
        { OracleDbType.NVarchar2, typeof(string) },
        { OracleDbType.Clob, typeof(string) },
        { OracleDbType.NClob, typeof(string) },
        { OracleDbType.Blob, typeof(byte[]) },
        { OracleDbType.BFile, typeof(byte[]) },
        { OracleDbType.Raw, typeof(byte[]) },
        { OracleDbType.Decimal, typeof(decimal) },
        { OracleDbType.Double, typeof(double) },
        { OracleDbType.Single, typeof(float) },
        { OracleDbType.Date, typeof(DateTime) },
        { OracleDbType.TimeStamp, typeof(DateTime) },
        { OracleDbType.TimeStampTZ, typeof(DateTimeOffset) },
        { OracleDbType.IntervalDS, typeof(TimeSpan) },
        { OracleDbType.Byte, typeof(byte) },
        { OracleDbType.XmlType, typeof(string) },
        { OracleDbType.RefCursor, typeof(object) },
        { OracleDbType.Json, typeof(string) },
        { OracleDbType.Long, typeof(long) }
    };

    /// <summary>
    /// SQL column types for supported strings
    /// </summary>
    public static readonly ReadOnlyCollection<OracleDbType> VariableCharacterColumnTypes = new ReadOnlyCollection<OracleDbType>(new List<OracleDbType> {
            OracleDbType.Varchar2,
            OracleDbType.NVarchar2,
            OracleDbType.Char,
            OracleDbType.NChar,
            OracleDbType.Blob,
            OracleDbType.Clob
        });

    public static readonly Dictionary<OracleDbType, int?> MaxSizeMap = new Dictionary<OracleDbType, int?>
    {
        { OracleDbType.Varchar2, 4000 },    // Max size in bytes for VARCHAR2
        { OracleDbType.NVarchar2, 2000 },   // Max size in bytes for NVARCHAR2
        { OracleDbType.Char, 2000 },        // Max size in bytes for CHAR
        { OracleDbType.NChar, 1000 },       // Max size in bytes for NCHAR
         { OracleDbType.Raw, 2000 }
    };

    /// <summary>
    /// The SQL column types which require a non-zero DataLength property.
    /// </summary>
    public static readonly List<OracleDbType> DataLengthRequired = new List<OracleDbType>
        {
            OracleDbType.Varchar2,
            OracleDbType.NVarchar2,
            OracleDbType.Char,
            OracleDbType.NChar,
            OracleDbType.Raw
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
            { typeof(string), OracleDbType.Varchar2 },
            { typeof(float), OracleDbType.Single },
            { typeof(short), OracleDbType.Int16 },
            { typeof(TimeSpan), OracleDbType.IntervalDS },
            { typeof(byte), OracleDbType.Byte },
            { typeof(Guid), OracleDbType.Raw },
            { typeof(char), OracleDbType.Char },
            { typeof(byte[]), OracleDbType.Blob },
            { typeof(char[]), OracleDbType.Clob }
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
        OracleDbType.Varchar2,
        OracleDbType.Char,
        OracleDbType.Raw,
        OracleDbType.XmlType
    };


}

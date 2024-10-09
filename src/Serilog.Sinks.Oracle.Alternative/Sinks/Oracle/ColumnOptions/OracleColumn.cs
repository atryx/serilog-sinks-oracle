﻿using System.Data;
using Oracle.ManagedDataAccess.Client;
using static System.FormattableString;

namespace Serilog.Sinks.Oracle;
public class OracleColumn
{
    private OracleDbType _dataType = OracleDbType.Varchar2; // backwards-compatibility default
    private string _columnName = string.Empty;
    private string _propertyName;
    private readonly List<string> _propertyNameHierarchy = new List<string>();

    /// <summary>
    /// Default constructor.
    /// </summary>
    public OracleColumn()
    { }

    /// <summary>
    /// Constructor with property initialization.
    /// </summary>
    public OracleColumn(string columnName, OracleDbType dataType, bool allowNull = true, int dataLength = -1)
    {
        ColumnName = columnName;
        DataType = dataType;
        AllowNull = allowNull;
        DataLength = dataLength;
    }

    /// <summary>
    /// A constructor that initializes the object from a DataColumn object.
    /// </summary>
    public OracleColumn(DataColumn dataColumn)
    {
        ColumnName = dataColumn?.ColumnName;
        AllowNull = dataColumn.AllowDBNull;

        if (!OracleDataTypes.ReverseTypeMap.ContainsKey(dataColumn.DataType))
            throw new ArgumentException(Invariant($".NET type {dataColumn.DataType} does not map to a supported SQL column data type."));

        DataType = OracleDataTypes.ReverseTypeMap[dataColumn.DataType];
        DataLength = dataColumn.MaxLength;

        if (DataLength == 0 && OracleDataTypes.DataLengthRequired.Contains(DataType))
            throw new ArgumentException(Invariant($".NET type {dataColumn.DataType} maps to a SQL column data type requiring a non-zero DataLength property."));
    }

    /// <summary>
    /// The name of the column in the database. Always required.
    /// </summary>
    public string ColumnName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_columnName) && StandardColumnIdentifier != null)
                return StandardColumnIdentifier.ToString();
            return _columnName;
        }
        set
        {
            _columnName = value;
        }
    }

    /// <summary>
    /// The SQL data type to be stored in this column. Always required.
    /// </summary>
    // Some Standard Columns hide this (via "new") to impose a more restricted list.
    public OracleDbType DataType
    {
        get => _dataType;
        set
        {
            if (!OracleDataTypes.SystemTypeMap.ContainsKey(value))
                throw new ArgumentException(Invariant($"SQL column data type {value} is not supported by this sink."));
            _dataType = value;
        }
    }

    /// <summary>
    /// Indicates whether NULLs can be stored in this column. Default is true. Always required.
    /// </summary>
    // The Id Standard Column hides this (via "new") to force this to false.
    public bool AllowNull { get; set; } = true;

    /// <summary>
    /// For character-storage DataTypes such as CHAR or VARCHAR, this defines the maximum size. The default -1 represents MAX.
    /// </summary>
    public int DataLength { get; set; } = -1;

    /// <summary>
    /// Determines whether a non-clustered index is created for this column. Compound indexes are not
    /// supported for auto-created log tables. This property is only used when auto-creating a log table.
    /// </summary>
    public bool NonClusteredIndex { get; set; }

    /// <summary>
    /// The name of the Serilog property to use as the value when filling the DataTable.
    /// If not specified, the ColumnName and PropertyName are the same.
    /// </summary>
    public string PropertyName
    {
        get => _propertyName ?? ColumnName;
        set
        {
            _propertyName = value;
            ParseHierarchicalPropertyName(value);
        }
    }

    public string DataTypeAsString {
        get => OracleDataTypes.SystemTypeMap[DataType];
        set
        {
            _propertyName = value;
        }
    }

    /// <summary>
    /// List of the hierachical parts of a property name and all sub properties (e.g. Property.Settings.EventName)
    /// </summary>
    internal IReadOnlyList<string> PropertyNameHierarchy
        => _propertyNameHierarchy;

    /// <summary>
    /// True if property name is hierarchical (e.g. Property.Settings.EventName)
    /// </summary>
    internal bool HasHierarchicalPropertyName
        => _propertyNameHierarchy.Count > 1;

    // Set by the constructors of the Standard Column classes that inherit from this;
    // allows Standard Columns and user-defined columns to coexist but remain identifiable
    // and allows casting back to the Standard Column without a lot of switch gymnastics.
    internal StandardColumn? StandardColumnIdentifier { get; set; }

    internal Type StandardColumnType { get; set; }

    /// <summary>
    /// Converts a SQL sink SqlColumn object to a System.Data.DataColumn object. The original
    /// SqlColumn object is stored in the DataColumn's ExtendedProperties collection.
    /// Virtual so that the Id Standard Column can perform additional configuration.
    /// </summary>
    internal virtual DataColumn AsDataColumn()
    {
        var dataColumn = new DataColumn
        {
            ColumnName = ColumnName,
            DataType = OracleDataTypes.OracleToCSharpTypeMap[DataType],
            AllowDBNull = AllowNull
        };

        if (OracleDataTypes.DataLengthRequired.Contains(DataType))
        {
            if (DataLength == 0)
                throw new ArgumentException(Invariant($"Column \"{ColumnName}\" is of type {DataType.ToString().ToUpperInvariant()} which requires a non-zero DataLength."));

            dataColumn.MaxLength = DataLength;
        }

        dataColumn.ExtendedProperties.Add("SqlColumn", this);
        return dataColumn;
    }

    /// <summary>
    /// Configuration accepts DataType as a simple string ("nvarchar" for example) for ease-of-use.
    /// This converts to SqlDbType and stores it into the DataType property.
    /// </summary>
    internal void SetDataTypeFromConfigString(string requestedSqlType)
    {
        if (!OracleDataTypes.TryParseIfSupported(requestedSqlType, out var oracleDbType))
            throw new ArgumentException(Invariant($"SQL column data type {requestedSqlType} is not recognized or not supported by this sink."));

        DataType = oracleDbType;
    }

    private void ParseHierarchicalPropertyName(string propertyName)
    {
        _propertyNameHierarchy.AddRange(propertyName.Split('.'));
    }
}

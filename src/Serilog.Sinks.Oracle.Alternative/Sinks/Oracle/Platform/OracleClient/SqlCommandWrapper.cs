﻿using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Serilog.Sinks.Oracle.Platform.SqlClient;

internal class SqlCommandWrapper : ISqlCommandWrapper
{
    private readonly OracleCommand _sqlCommand;
    private bool _disposedValue;

    public SqlCommandWrapper(OracleCommand sqlCommand)
    {
        _sqlCommand = sqlCommand ?? throw new ArgumentNullException(nameof(sqlCommand));
    }

    public CommandType CommandType
    {
        get => _sqlCommand.CommandType;
        set => _sqlCommand.CommandType = value;
    }

    public string CommandText
    {
        get => _sqlCommand.CommandText;
        set => _sqlCommand.CommandText = value;
    }

    public void AddParameter(string parameterName, object value)
    {
        var parameter = new OracleParameter(parameterName, value ?? DBNull.Value);

        // The default is SqlDbType.DateTime, which will truncate the DateTime value if the actual
        // type in the database table is datetime2. So we explicitly set it to DateTime2, which will
        // work both if the field in the table is datetime and datetime2, which is also consistent with 
        // the behavior of the non-audit sink.
        if (value is DateTime)
            parameter.OracleDbType = OracleDbType.TimeStamp;

        _sqlCommand.Parameters.Add(parameter);
    }

    public int ExecuteNonQuery() =>
        _sqlCommand.ExecuteNonQuery();

    public Task<int> ExecuteNonQueryAsync() =>
        _sqlCommand.ExecuteNonQueryAsync();

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _sqlCommand.Dispose();
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

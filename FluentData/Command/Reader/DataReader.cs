using System;
using System.Data;

namespace FluentData
{
    /// <summary>
    /// Wraps an ADO.NET IDataReader with additional convenience methods for reading data by column name or index.
    /// </summary>
    internal class DataReader : IDataReader
    {
        /// <summary>
        /// Gets the underlying ADO.NET IDataReader.
        /// </summary>
        public System.Data.IDataReader InnerReader { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DataReader"/>.
        /// </summary>
        /// <param name="reader">The ADO.NET IDataReader to wrap.</param>
        public DataReader(System.Data.IDataReader reader)
        {
            InnerReader = reader;
        }

        /// <summary>
        /// Closes the underlying data reader.
        /// </summary>
        public void Close()
        {
            InnerReader.Close();
        }

        private T GetValue<T>(int i)
        {
            var value = InnerReader.GetValue(i);
            if (value == DBNull.Value)
                return default;
            return (T)value;
        }

        /// <summary>
        /// Gets the depth of nesting for the current row.
        /// </summary>
        public int Depth
        {
            get { return InnerReader.Depth; }
        }

        /// <summary>
        /// Returns a DataTable that describes the column metadata of the DataReader.
        /// </summary>
        /// <returns>A DataTable describing the column metadata.</returns>
        public DataTable GetSchemaTable()
        {
            return InnerReader.GetSchemaTable();
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        public bool IsClosed
        {
            get { return InnerReader.IsClosed; }
        }

        /// <summary>
        /// Advances the data reader to the next result set.
        /// </summary>
        /// <returns>True if there are more result sets; otherwise, false.</returns>
        public bool NextResult()
        {
            return InnerReader.NextResult();
        }

        /// <summary>
        /// Advances the data reader to the next record.
        /// </summary>
        /// <returns>True if there are more rows; otherwise, false.</returns>
        public bool Read()
        {
            return InnerReader.Read();
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        public int RecordsAffected
        {
            get { return InnerReader.RecordsAffected; }
        }

        /// <summary>
        /// Releases the resources used by the data reader.
        /// </summary>
        public void Dispose()
        {
            InnerReader.Dispose();
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        public int FieldCount
        {
            get { return InnerReader.FieldCount; }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public bool GetBoolean(int i)
        {
            return GetValue<bool>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public bool GetBoolean(string name)
        {
            return GetBoolean(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public byte GetByte(int i)
        {
            return GetValue<byte>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a byte.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public byte GetByte(string name)
        {
            return GetByte(GetOrdinal(name));
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for the buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of bytes read.</returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return IsDBNull(i) ? 0 : InnerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="fieldOffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for the buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of bytes read.</returns>
        public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return GetBytes(GetOrdinal(name), fieldOffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets the value of the specified column as a character.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public char GetChar(int i)
        {
            return GetValue<char>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a character.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public char GetChar(string name)
        {
            return GetChar(GetOrdinal(name));
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of characters.</param>
        /// <param name="bufferoffset">The index for the buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of characters read.</returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return IsDBNull(i) ? 0 : InnerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <param name="fieldoffset">The index within the field from which to begin the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of characters.</param>
        /// <param name="bufferoffset">The index for the buffer to begin the read operation.</param>
        /// <param name="length">The maximum length to copy into the buffer.</param>
        /// <returns>The actual number of characters read.</returns>
        public long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return GetChars(GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets an IDataReader for the specified column ordinal.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>An IDataReader for the column.</returns>
        public System.Data.IDataReader GetData(int i)
        {
            return InnerReader.GetData(i);
        }

        /// <summary>
        /// Gets an IDataReader for the specified column name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>An IDataReader for the column.</returns>
        public System.Data.IDataReader GetData(string name)
        {
            return GetData(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the name of the data type of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The name of the data type.</returns>
        public string GetDataTypeName(int i)
        {
            return InnerReader.GetDataTypeName(i);
        }

        /// <summary>
        /// Gets the name of the data type of the specified column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The name of the data type.</returns>
        public string GetDataTypeName(string name)
        {
            return GetDataTypeName(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public DateTime GetDateTime(int i)
        {
            return GetValue<DateTime>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a DateTime.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public DateTime GetDateTime(string name)
        {
            return GetDateTime(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a Decimal.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public decimal GetDecimal(int i)
        {
            return GetValue<decimal>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Decimal.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public decimal GetDecimal(string name)
        {
            return GetDecimal(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a Double.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public double GetDouble(int i)
        {
            return GetValue<double>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Double.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public double GetDouble(string name)
        {
            return GetDouble(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the Type that is the data type of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The data type of the column.</returns>
        public Type GetFieldType(int i)
        {
            return InnerReader.GetFieldType(i);
        }

        /// <summary>
        /// Gets the Type that is the data type of the specified column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The data type of the column.</returns>
        public Type GetFieldType(string name)
        {
            return GetFieldType(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a Single.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public float GetFloat(int i)
        {
            return GetValue<float>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Single.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public float GetFloat(string name)
        {
            return GetFloat(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a Guid.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public Guid GetGuid(int i)
        {
            return GetValue<Guid>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a Guid.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public Guid GetGuid(string name)
        {
            return GetGuid(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as an Int16.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public short GetInt16(int i)
        {
            return GetValue<short>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an Int16.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public short GetInt16(string name)
        {
            return GetInt16(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as an Int32.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public int GetInt32(int i)
        {
            return GetValue<int>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an Int32.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public int GetInt32(string name)
        {
            return GetInt32(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as an Int64.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public long GetInt64(int i)
        {
            return GetValue<long>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an Int64.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public long GetInt64(string name)
        {
            return GetInt64(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The name of the column.</returns>
        public string GetName(int i)
        {
            return InnerReader.GetName(i);
        }

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The name of the column.</returns>
        public string GetName(string name)
        {
            return InnerReader.GetName(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the column ordinal, given the name of the column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The zero-based column ordinal.</returns>
        public int GetOrdinal(string name)
        {
            return InnerReader.GetOrdinal(name);
        }

        /// <summary>
        /// Gets the value of the specified column as a String.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public string GetString(int i)
        {
            return GetValue<string>(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a String.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public string GetString(string name)
        {
            return GetString(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public object GetValue(int i)
        {
            return GetValue<object>(i);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public object GetValue(string name)
        {
            return GetValue(GetOrdinal(name));
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current row.
        /// </summary>
        /// <param name="values">An array of Object into which to copy the attribute columns.</param>
        /// <returns>The number of instances of Object in the array.</returns>
        public int GetValues(object[] values)
        {
            return InnerReader.GetValues(values);
        }

        /// <summary>
        /// Gets whether the specified column contains null values.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>True if the column is null; otherwise, false.</returns>
        public bool IsDBNull(int i)
        {
            return InnerReader.IsDBNull(i);
        }

        /// <summary>
        /// Gets whether the specified column contains null values.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>True if the column is null; otherwise, false.</returns>
        public bool IsDBNull(string name)
        {
            return IsDBNull(GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column by name.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>The value of the column.</returns>
        public object this[string name]
        {
            get { return this[GetOrdinal(name)]; }
        }

        /// <summary>
        /// Gets the value of the specified column by index.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column, or null if DBNull.</returns>
        public object this[int i] => IsDBNull(i) ? null : InnerReader[i];
    }
}

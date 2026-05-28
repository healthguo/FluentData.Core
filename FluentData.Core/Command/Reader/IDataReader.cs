namespace FluentData.Core
{
    /// <summary>
    /// Lightweight wrapper around <see cref="System.Data.IDataReader"/> that provides
    /// convenient helpers for accessing column values by column name instead of ordinal.
    /// Implementations adapt an underlying ADO.NET data reader and expose a simplified API
    /// used throughout the FluentData library.
    /// </summary>
    /// <remarks>
    /// Methods that accept a column <paramref name="name"/> should resolve the column ordinal
    /// internally. Callers can expect name lookups to be case-insensitive when the underlying
    /// implementation supports it.
    /// </remarks>
    public interface IDataReader : System.Data.IDataReader
    {
        /// <summary>
        /// Gets the underlying ADO.NET data reader wrapped by this instance.
        /// </summary>
        System.Data.IDataReader InnerReader { get; }

        /// <summary>
        /// Gets a boolean value from the column identified by <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Column name to read. Must not be null or empty.</param>
        /// <returns>The boolean value of the specified column.</returns>
        bool GetBoolean(string name);

        /// <summary>
        /// Gets a byte value from the column identified by <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The byte value of the specified column.</returns>
        byte GetByte(string name);

        /// <summary>
        /// Reads a stream of bytes from the specified column into a buffer.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The destination buffer.</param>
        /// <param name="bufferoffset">The zero-based index in <paramref name="buffer"/> where copying begins.</param>
        /// <param name="length">The maximum number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length);

        /// <summary>
        /// Gets a single Unicode character from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The character value.</returns>
        char GetChar(string name);

        /// <summary>
        /// Reads a stream of characters from the specified column into a buffer.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <param name="fieldoffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The destination character buffer.</param>
        /// <param name="bufferoffset">The zero-based index in <paramref name="buffer"/> where copying begins.</param>
        /// <param name="length">The maximum number of characters to read.</param>
        /// <returns>The actual number of characters read.</returns>
        long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length);

        /// <summary>
        /// Gets the name of the data type for the specified column.
        /// </summary>
        /// <param name="name">Column name to inspect.</param>
        /// <returns>The provider-specific data type name of the column.</returns>
        string GetDataTypeName(string name);

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The <see cref="DateTime"/> value.</returns>
        DateTime GetDateTime(string name);

        /// <summary>
        /// Gets a <see cref="decimal"/> value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The decimal value.</returns>
        decimal GetDecimal(string name);

        /// <summary>
        /// Gets a <see cref="double"/> value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The double value.</returns>
        double GetDouble(string name);

        /// <summary>
        /// Gets the CLR <see cref="Type"/> of the specified column.
        /// </summary>
        /// <param name="name">Column name to inspect.</param>
        /// <returns>The column CLR type.</returns>
        Type GetFieldType(string name);

        /// <summary>
        /// Gets a <see cref="float"/> value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The float value.</returns>
        float GetFloat(string name);

        /// <summary>
        /// Gets a <see cref="Guid"/> value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The GUID value.</returns>
        Guid GetGuid(string name);

        /// <summary>
        /// Gets a 16-bit integer from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The 16-bit integer value.</returns>
        short GetInt16(string name);

        /// <summary>
        /// Gets a 32-bit integer from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The 32-bit integer value.</returns>
        int GetInt32(string name);

        /// <summary>
        /// Gets a 64-bit integer from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The 64-bit integer value.</returns>
        long GetInt64(string name);

        /// <summary>
        /// Gets the column name for the given column name (identity function included for compatibility).
        /// </summary>
        /// <param name="name">Column name to retrieve.</param>
        /// <returns>The resolved column name.</returns>
        string GetName(string name);

        /// <summary>
        /// Gets a string value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The string value.</returns>
        string GetString(string name);

        /// <summary>
        /// Gets the raw object value from the specified column.
        /// </summary>
        /// <param name="name">Column name to read.</param>
        /// <returns>The raw value; may be <c>DBNull.Value</c> for database NULLs.</returns>
        object GetValue(string name);

        /// <summary>
        /// Returns whether the specified column contains a database NULL value.
        /// </summary>
        /// <param name="name">Column name to check.</param>
        /// <returns><c>true</c> if the column value is <c>DBNull</c>; otherwise <c>false</c>.</returns>
        bool IsDBNull(string name);
    }
}

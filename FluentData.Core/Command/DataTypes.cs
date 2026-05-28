namespace FluentData.Core
{
    /// <summary>
    /// Specifies the database data types supported by FluentData for parameter mapping.
    /// </summary>
    public enum DataTypes
    {
        /// <summary>
        /// A variable-length stream of non-Unicode characters ranging between 1 and 8,000 characters.
        /// </summary>
        AnsiString = 0,

        /// <summary>
        /// A variable-length stream of binary data ranging between 1 and 8,000 bytes.
        /// </summary>
        Binary = 1,

        /// <summary>
        /// An 8-bit unsigned integer ranging in value from 0 to 255.
        /// </summary>
        Byte = 2,

        /// <summary>
        /// A simple type representing Boolean values of true or false.
        /// </summary>
        Boolean = 3,

        /// <summary>
        /// A currency value ranging from -2^63 to 2^63 - 1 with an accuracy to a ten-thousandth of a currency unit.
        /// </summary>
        Currency = 4,

        /// <summary>
        /// A type representing a date value.
        /// </summary>
        Date = 5,

        /// <summary>
        /// A type representing a date and time value.
        /// </summary>
        DateTime = 6,

        /// <summary>
        /// A simple type representing values ranging from 1.0 x 10^-28 to approximately 7.9 x 10^28 with 28-29 significant digits.
        /// </summary>
        Decimal = 7,

        /// <summary>
        /// A floating point type representing values ranging from approximately 5.0 x 10^-324 to 1.7 x 10^308 with a precision of 15-16 digits.
        /// </summary>
        Double = 8,

        /// <summary>
        /// A globally unique identifier (or GUID).
        /// </summary>
        Guid = 9,

        /// <summary>
        /// An integral type representing signed 16-bit integers with values between -32768 and 32767.
        /// </summary>
        Int16 = 10,

        /// <summary>
        /// An integral type representing signed 32-bit integers with values between -2147483648 and 2147483647.
        /// </summary>
        Int32 = 11,

        /// <summary>
        /// An integral type representing signed 64-bit integers with values between -9223372036854775808 and 9223372036854775807.
        /// </summary>
        Int64 = 12,

        /// <summary>
        /// A general type representing any reference or value type not explicitly represented by another DataTypes value.
        /// </summary>
        Object = 13,

        /// <summary>
        /// An integral type representing signed 8-bit integers with values between -128 and 127.
        /// </summary>
        SByte = 14,

        /// <summary>
        /// A floating point type representing values ranging from approximately 1.5 x 10^-45 to 3.4 x 10^38 with a precision of 7 digits.
        /// </summary>
        Single = 15,

        /// <summary>
        /// A type representing Unicode character strings.
        /// </summary>
        String = 16,

        /// <summary>
        /// A type representing a SQL Server DateTime value.
        /// </summary>
        Time = 17,

        /// <summary>
        /// An integral type representing unsigned 16-bit integers with values between 0 and 65535.
        /// </summary>
        UInt16 = 18,

        /// <summary>
        /// An integral type representing unsigned 32-bit integers with values between 0 and 4294967295.
        /// </summary>
        UInt32 = 19,

        /// <summary>
        /// An integral type representing unsigned 64-bit integers with values between 0 and 18446744073709551615.
        /// </summary>
        UInt64 = 20,

        /// <summary>
        /// A variable-length numeric value.
        /// </summary>
        VarNumeric = 21,

        /// <summary>
        /// A fixed-length stream of non-Unicode characters.
        /// </summary>
        AnsiStringFixedLength = 22,

        /// <summary>
        /// A fixed-length string of Unicode characters.
        /// </summary>
        StringFixedLength = 23,

        /// <summary>
        /// A parsed representation of an XML document or fragment.
        /// </summary>
        Xml = 25,

        /// <summary>
        /// Date and time data. Date value range is from January 1, 1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds.
        /// </summary>
        DateTime2 = 26,

        /// <summary>
        /// Date and time data with time zone awareness. Date value range is from January 1, 1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range is -14:00 through +14:00.
        /// </summary>
        DateTimeOffset = 27,
    }
}

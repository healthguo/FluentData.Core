namespace FluentData
{
    /// <summary>
    /// Specifies the type of database command to execute.
    /// </summary>
    public enum DbCommandTypes
    {
        /// <summary>
        /// An SQL text command. This is the default type.
        /// </summary>
        Text = 1,

        /// <summary>
        /// The name of a stored procedure.
        /// </summary>
        StoredProcedure = 4,

        /// <summary>
        /// The name of a table for direct access.
        /// </summary>
        TableDirect = 512,
    }
}

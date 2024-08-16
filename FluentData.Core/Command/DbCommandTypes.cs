namespace FluentData.Core
{
    public enum DbCommandTypes
    {
        //
        // 摘要:
        //     An SQL text command. (Default.)
        Text = 1,
        //
        // 摘要:
        //     The name of a stored procedure.
        StoredProcedure = 4,
        //
        // 摘要:
        //     The name of a table.
        TableDirect = 512,
    }
}

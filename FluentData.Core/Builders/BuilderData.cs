namespace FluentData.Core
{
    /// <summary>
    /// Contains data for building INSERT, UPDATE, DELETE, and stored procedure commands.
    /// </summary>
    public class BuilderData
    {
        /// <summary>
        /// Gets or sets the list of columns to be included in the command.
        /// </summary>
        public List<BuilderColumn> Columns { get; set; }

        /// <summary>
        /// Gets or sets the entity item containing values for the command.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the name of the database object (table or stored procedure).
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDbCommand"/> associated with this builder.
        /// </summary>
        public IDbCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the list of WHERE clause columns.
        /// </summary>
        public List<BuilderColumn> Where { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BuilderData"/>.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to associate with this builder.</param>
        /// <param name="objectName">The name of the database object (table or stored procedure).</param>
        public BuilderData(IDbCommand command, string objectName)
        {
            ObjectName = objectName;
            Command = command;
            Columns = new List<BuilderColumn>();
            Where = new List<BuilderColumn>();
        }
    }
}

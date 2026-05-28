namespace FluentData
{
    /// <summary>
    /// Contains data for building SELECT queries, extending <see cref="BuilderData"/> with SELECT-specific properties.
    /// </summary>
    public class SelectBuilderData : BuilderData
    {
        /// <summary>
        /// Gets or sets the current page number for paging (1-based).
        /// </summary>
        public int PagingCurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page for paging.
        /// </summary>
        public int PagingItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the HAVING clause for the SELECT query.
        /// </summary>
        public string Having { get; set; }

        /// <summary>
        /// Gets or sets the GROUP BY clause for the SELECT query.
        /// </summary>
        public string GroupBy { get; set; }

        /// <summary>
        /// Gets or sets the ORDER BY clause for the SELECT query.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets the FROM clause for the SELECT query.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the SELECT columns clause.
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Gets or sets the WHERE clause SQL for the SELECT query.
        /// </summary>
        public string WhereSql { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="SelectBuilderData"/>.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> to associate with this builder.</param>
        /// <param name="objectName">The name of the table or view to query.</param>
        public SelectBuilderData(IDbCommand command, string objectName) : base(command, objectName)
        {
            Having = "";
            GroupBy = "";
            OrderBy = "";
            From = "";
            Select = "";
            WhereSql = "";
            PagingCurrentPage = 1;
            PagingItemsPerPage = 0;
        }

        /// <summary>
        /// Calculates the starting row number for the current page.
        /// </summary>
        /// <returns>The 1-based starting row number.</returns>
        internal int GetFromItems()
        {
            return (GetToItems() - PagingItemsPerPage + 1);
        }

        /// <summary>
        /// Calculates the ending row number for the current page.
        /// </summary>
        /// <returns>The 1-based ending row number.</returns>
        internal int GetToItems()
        {
            return (PagingCurrentPage * PagingItemsPerPage);
        }
    }
}

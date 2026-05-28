using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Handles query result mapping for <see cref="DataTable"/> type.
    /// Loads the entire data reader result into a DataTable.
    /// </summary>
    /// <typeparam name="TEntity">The entity type (must be DataTable).</typeparam>
    internal class QueryDataTableHandler<TEntity> : IQueryTypeHandler<TEntity>
    {
        private readonly DbCommandData _data;

        /// <summary>
        /// Gets a value indicating whether to iterate through the data reader row by row.
        /// Always returns false for DataTable handlers since DataTable.Load handles iteration.
        /// </summary>
        public bool IterateDataReader { get { return false; } }

        /// <summary>
        /// Creates a new instance of <see cref="QueryDataTableHandler{TEntity}"/>.
        /// </summary>
        /// <param name="data">The command data containing execution context.</param>
        public QueryDataTableHandler(DbCommandData data)
        {
            _data = data;
        }

        /// <summary>
        /// Loads the entire data reader result into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="customMapperReader">Unused for DataTable handlers.</param>
        /// <param name="customMapperDynamic">Unused for DataTable handlers.</param>
        /// <returns>A <see cref="DataTable"/> containing all query results.</returns>
        public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
        {
            var dataTable = new DataTable();
            dataTable.Load(_data.Reader.InnerReader, LoadOption.OverwriteChanges);
            return dataTable;
        }
    }
}

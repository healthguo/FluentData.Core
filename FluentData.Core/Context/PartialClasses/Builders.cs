using System.Dynamic;

namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Creates a new <see cref="ISelectBuilder{TEntity}"/> for building SELECT queries.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map results to.</typeparam>
        /// <param name="sql">The initial SQL text for the SELECT statement.</param>
        /// <returns>A new <see cref="ISelectBuilder{TEntity}"/> instance for method chaining.</returns>
        public ISelectBuilder<TEntity> Select<TEntity>(string sql)
        {
            return new SelectBuilder<TEntity>(CreateCommand).Select(sql);
        }

        /// <summary>
        /// Creates a new <see cref="IInsertBuilder"/> for building INSERT statements.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <returns>A new <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        public IInsertBuilder Insert(string tableName)
        {
            return new InsertBuilder(CreateCommand, tableName);
        }

        /// <summary>
        /// Creates a new <see cref="IInsertBuilder{T}"/> for building INSERT statements with a strongly-typed entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity to insert.</typeparam>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="item">The entity object containing values to insert.</param>
        /// <returns>A new <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        public IInsertBuilder<T> Insert<T>(string tableName, T item)
        {
            return new InsertBuilder<T>(CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IInsertBuilder{T}"/> for building INSERT statements, inferring the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to insert.</typeparam>
        /// <param name="item">The entity object containing values to insert.</param>
        /// <returns>A new <see cref="IInsertBuilder{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.</exception>
        public IInsertBuilder<T> Insert<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var tableName = item.GetType().Name;
            return this.Insert(tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IInsertBuilderDynamic"/> for building INSERT statements with a dynamic entity.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="item">The <see cref="ExpandoObject"/> containing values to insert.</param>
        /// <returns>A new <see cref="IInsertBuilderDynamic"/> instance for method chaining.</returns>
        public IInsertBuilderDynamic Insert(string tableName, ExpandoObject item)
        {
            return new InsertBuilderDynamic(CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IUpdateBuilder"/> for building UPDATE statements.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <returns>A new <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        public IUpdateBuilder Update(string tableName)
        {
            return new UpdateBuilder(Data.FluentDataProvider, CreateCommand, tableName);
        }

        /// <summary>
        /// Creates a new <see cref="IUpdateBuilder{T}"/> for building UPDATE statements with a strongly-typed entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity to update.</typeparam>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="item">The entity object containing values to update.</param>
        /// <returns>A new <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        public IUpdateBuilder<T> Update<T>(string tableName, T item)
        {
            return new UpdateBuilder<T>(Data.FluentDataProvider, CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IUpdateBuilder{T}"/> for building UPDATE statements, inferring the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to update.</typeparam>
        /// <param name="item">The entity object containing values to update.</param>
        /// <returns>A new <see cref="IUpdateBuilder{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.</exception>
        public IUpdateBuilder<T> Update<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var tableName = item.GetType().Name;
            return this.Update(tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IUpdateBuilderDynamic"/> for building UPDATE statements with a dynamic entity.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="item">The <see cref="ExpandoObject"/> containing values to update.</param>
        /// <returns>A new <see cref="IUpdateBuilderDynamic"/> instance for method chaining.</returns>
        public IUpdateBuilderDynamic Update(string tableName, ExpandoObject item)
        {
            return new UpdateBuilderDynamic(Data.FluentDataProvider, CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IDeleteBuilder"/> for building DELETE statements.
        /// </summary>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <returns>A new <see cref="IDeleteBuilder"/> instance for method chaining.</returns>
        public IDeleteBuilder Delete(string tableName)
        {
            return new DeleteBuilder(CreateCommand, tableName);
        }

        /// <summary>
        /// Creates a new <see cref="IDeleteBuilder{T}"/> for building DELETE statements with a strongly-typed entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity used for WHERE clause values.</typeparam>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <param name="item">The entity object containing values for the WHERE clause.</param>
        /// <returns>A new <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        public IDeleteBuilder<T> Delete<T>(string tableName, T item)
        {
            return new DeleteBuilder<T>(CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IDeleteBuilder{T}"/> for building DELETE statements, inferring the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity used for WHERE clause values.</typeparam>
        /// <param name="item">The entity object containing values for the WHERE clause.</param>
        /// <returns>A new <see cref="IDeleteBuilder{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.</exception>
        public IDeleteBuilder<T> Delete<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var tableName = item.GetType().Name;
            return this.Delete(tableName, item);
        }

        /// <summary>
        /// Verifies that the current provider supports stored procedures.
        /// </summary>
        /// <exception cref="FluentDataException">Thrown if the provider does not support stored procedures.</exception>
        private void VerifyStoredProcedureSupport()
        {
            if (!Data.FluentDataProvider.SupportsStoredProcedures)
                throw new FluentDataException("The selected database does not support stored procedures.");
        }

        /// <summary>
        /// Creates a new <see cref="IStoredProcedureBuilder"/> for executing stored procedures.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <returns>A new <see cref="IStoredProcedureBuilder"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the provider does not support stored procedures.</exception>
        public IStoredProcedureBuilder StoredProcedure(string storedProcedureName)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilder(CreateCommand, storedProcedureName);
        }

        /// <summary>
        /// Creates a new <see cref="IStoredProcedureBuilder{T}"/> for executing stored procedures with a strongly-typed entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity containing parameter values.</typeparam>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="item">The entity object containing parameter values.</param>
        /// <returns>A new <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the provider does not support stored procedures.</exception>
        public IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilder<T>(CreateCommand, storedProcedureName, item);
        }

        /// <summary>
        /// Creates a new <see cref="IStoredProcedureBuilderDynamic"/> for executing stored procedures with a dynamic entity.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="item">The <see cref="ExpandoObject"/> containing parameter values.</param>
        /// <returns>A new <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the provider does not support stored procedures.</exception>
        public IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilderDynamic(CreateCommand, storedProcedureName, item);
        }
    }
}

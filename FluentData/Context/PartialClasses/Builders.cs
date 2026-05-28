using System;
using System.Dynamic;

namespace FluentData
{
    public partial class DbContext
    {
        /// <summary>
        /// Creates a SELECT query builder for constructing SQL SELECT statements.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map query results to.</typeparam>
        /// <param name="sql">The initial SQL SELECT statement.</param>
        /// <returns>An <see cref="ISelectBuilder{TEntity}"/> for building the query.</returns>
        public ISelectBuilder<TEntity> Select<TEntity>(string sql)
        {
            return new SelectBuilder<TEntity>(CreateCommand).Select(sql);
        }

        /// <summary>
        /// Creates a non-generic INSERT builder for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <returns>An <see cref="IInsertBuilder"/> for building the INSERT command.</returns>
        public IInsertBuilder Insert(string tableName)
        {
            return new InsertBuilder(CreateCommand, tableName);
        }

        /// <summary>
        /// Creates a strongly-typed INSERT builder for the specified table and entity.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to insert.</typeparam>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="item">The entity containing values to insert.</param>
        /// <returns>An <see cref="IInsertBuilder{T}"/> for building the INSERT command.</returns>
        public IInsertBuilder<T> Insert<T>(string tableName, T item)
        {
            return new InsertBuilder<T>(CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a strongly-typed INSERT builder, automatically deriving the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to insert.</typeparam>
        /// <param name="item">The entity containing values to insert.</param>
        /// <returns>An <see cref="IInsertBuilder{T}"/> for building the INSERT command.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the item is null.</exception>
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
        /// Creates a dynamic INSERT builder using <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="item">The dynamic object containing values to insert.</param>
        /// <returns>An <see cref="IInsertBuilderDynamic"/> for building the INSERT command.</returns>
        public IInsertBuilderDynamic Insert(string tableName, ExpandoObject item)
        {
            return new InsertBuilderDynamic(CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a non-generic UPDATE builder for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <returns>An <see cref="IUpdateBuilder"/> for building the UPDATE command.</returns>
        public IUpdateBuilder Update(string tableName)
        {
            return new UpdateBuilder(Data.FluentDataProvider, CreateCommand, tableName);
        }

        /// <summary>
        /// Creates a strongly-typed UPDATE builder for the specified table and entity.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to update.</typeparam>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="item">The entity containing values to update.</param>
        /// <returns>An <see cref="IUpdateBuilder{T}"/> for building the UPDATE command.</returns>
        public IUpdateBuilder<T> Update<T>(string tableName, T item)
        {
            return new UpdateBuilder<T>(Data.FluentDataProvider, CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a strongly-typed UPDATE builder, automatically deriving the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to update.</typeparam>
        /// <param name="item">The entity containing values to update.</param>
        /// <returns>An <see cref="IUpdateBuilder{T}"/> for building the UPDATE command.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the item is null.</exception>
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
        /// Creates a dynamic UPDATE builder using <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="item">The dynamic object containing values to update.</param>
        /// <returns>An <see cref="IUpdateBuilderDynamic"/> for building the UPDATE command.</returns>
        public IUpdateBuilderDynamic Update(string tableName, ExpandoObject item)
        {
            return new UpdateBuilderDynamic(Data.FluentDataProvider, CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a non-generic DELETE builder for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <returns>An <see cref="IDeleteBuilder"/> for building the DELETE command.</returns>
        public IDeleteBuilder Delete(string tableName)
        {
            return new DeleteBuilder(CreateCommand, tableName);
        }

        /// <summary>
        /// Creates a strongly-typed DELETE builder for the specified table and entity.
        /// </summary>
        /// <typeparam name="T">The entity type containing WHERE clause values.</typeparam>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <param name="item">The entity containing WHERE clause values.</param>
        /// <returns>An <see cref="IDeleteBuilder{T}"/> for building the DELETE command.</returns>
        public IDeleteBuilder<T> Delete<T>(string tableName, T item)
        {
            return new DeleteBuilder<T>(CreateCommand, tableName, item);
        }

        /// <summary>
        /// Creates a strongly-typed DELETE builder, automatically deriving the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The entity type containing WHERE clause values.</typeparam>
        /// <param name="item">The entity containing WHERE clause values.</param>
        /// <returns>An <see cref="IDeleteBuilder{T}"/> for building the DELETE command.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the item is null.</exception>
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
        /// <exception cref="FluentDataException">Thrown if stored procedures are not supported.</exception>
        private void VerifyStoredProcedureSupport()
        {
            if (!Data.FluentDataProvider.SupportsStoredProcedures)
                throw new FluentDataException("The selected database does not support stored procedures.");
        }

        /// <summary>
        /// Creates a non-generic stored procedure builder.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure.</param>
        /// <returns>An <see cref="IStoredProcedureBuilder"/> for executing the stored procedure.</returns>
        public IStoredProcedureBuilder StoredProcedure(string storedProcedureName)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilder(CreateCommand, storedProcedureName);
        }

        /// <summary>
        /// Creates a strongly-typed stored procedure builder.
        /// </summary>
        /// <typeparam name="T">The entity type containing parameter values.</typeparam>
        /// <param name="storedProcedureName">The name of the stored procedure.</param>
        /// <param name="item">The entity containing parameter values.</param>
        /// <returns>An <see cref="IStoredProcedureBuilder{T}"/> for executing the stored procedure.</returns>
        public IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilder<T>(CreateCommand, storedProcedureName, item);
        }

        /// <summary>
        /// Creates a dynamic stored procedure builder using <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure.</param>
        /// <param name="item">The dynamic object containing parameter values.</param>
        /// <returns>An <see cref="IStoredProcedureBuilderDynamic"/> for executing the stored procedure.</returns>
        public IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilderDynamic(CreateCommand, storedProcedureName, item);
        }
    }
}

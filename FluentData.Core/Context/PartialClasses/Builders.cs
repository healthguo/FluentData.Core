using System.Dynamic;

namespace FluentData.Core
{
    public partial class DbContext
    {
        public ISelectBuilder<TEntity> Select<TEntity>(string sql)
        {
            return new SelectBuilder<TEntity>(CreateCommand).Select(sql);
        }

        public IInsertBuilder Insert(string tableName)
        {
            return new InsertBuilder(CreateCommand, tableName);
        }

        public IInsertBuilder<T> Insert<T>(string tableName, T item)
        {
            return new InsertBuilder<T>(CreateCommand, tableName, item);
        }

        public IInsertBuilder<T> Insert<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var tableName = item.GetType().Name;
            return this.Insert(tableName, item);
        }

        public IInsertBuilderDynamic Insert(string tableName, ExpandoObject item)
        {
            return new InsertBuilderDynamic(CreateCommand, tableName, item);
        }

        public IUpdateBuilder Update(string tableName)
        {
            return new UpdateBuilder(Data.FluentDataProvider, CreateCommand, tableName);
        }

        public IUpdateBuilder<T> Update<T>(string tableName, T item)
        {
            return new UpdateBuilder<T>(Data.FluentDataProvider, CreateCommand, tableName, item);
        }

        public IUpdateBuilder<T> Update<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var tableName = item.GetType().Name;
            return this.Update(tableName, item);
        }

        public IUpdateBuilderDynamic Update(string tableName, ExpandoObject item)
        {
            return new UpdateBuilderDynamic(Data.FluentDataProvider, CreateCommand, tableName, item);
        }

        public IDeleteBuilder Delete(string tableName)
        {
            return new DeleteBuilder(CreateCommand, tableName);
        }

        public IDeleteBuilder<T> Delete<T>(string tableName, T item)
        {
            return new DeleteBuilder<T>(CreateCommand, tableName, item);
        }

        public IDeleteBuilder<T> Delete<T>(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var tableName = item.GetType().Name;
            return this.Delete(tableName, item);
        }

        private void VerifyStoredProcedureSupport()
        {
            if (!Data.FluentDataProvider.SupportsStoredProcedures)
                throw new FluentDataException("The selected database does not support stored procedures.");
        }

        public IStoredProcedureBuilder StoredProcedure(string storedProcedureName)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilder(CreateCommand, storedProcedureName);
        }

        public IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilder<T>(CreateCommand, storedProcedureName, item);
        }

        public IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item)
        {
            VerifyStoredProcedureSupport();
            return new StoredProcedureBuilderDynamic(CreateCommand, storedProcedureName, item);
        }
    }
}

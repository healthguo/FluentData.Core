using System.Data;

namespace FluentData.Core
{
    internal partial class DbCommand
    {
        public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                if (Data.Reader.Read())
                    item = customMapper(Data.Reader);
            }, typeof(TEntity) != typeof(DataTable));

            return item;
        }

        public TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                if (Data.Reader.Read())
                    item = customMapper(new DynamicDataReader(Data.Reader));
            }, typeof(TEntity) != typeof(DataTable));

            return item;
        }
    }
}

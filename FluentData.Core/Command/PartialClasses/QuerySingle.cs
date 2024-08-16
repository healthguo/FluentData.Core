using System.Data;

namespace FluentData.Core
{
    internal partial class DbCommand
    {
        public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader>? customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                item = new QueryHandler<TEntity>(Data).ExecuteSingle(customMapper, null);
            }, typeof(TEntity) != typeof(DataTable));

            return item!;
        }

        public TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            var item = default(TEntity);

            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                item = new QueryHandler<TEntity>(Data).ExecuteSingle(customMapper, null);
            }, typeof(TEntity) != typeof(DataTable));

            return item!;
        }
    }
}
using System.Data;

namespace FluentData.Core
{
    internal partial class DbCommand
    {
        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                while (Data.Reader.Read())
                    customMapper(list, Data.Reader);
            }, typeof(TEntity) != typeof(DataTable));
        }

        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            Data.ExecuteQueryHandler.ExecuteQuery(true, () =>
            {
                while (Data.Reader.Read())
                    customMapper(list, Data.Reader);
            }, typeof(TEntity) != typeof(DataTable));
        }
    }
}

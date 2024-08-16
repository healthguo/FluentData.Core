using System.Data;

namespace FluentData.Core
{
    internal abstract class BaseStoredProcedureBuilder : IBaseStoredProcedureBuilder
    {
        public BuilderData Data { get; set; }

        protected ActionsHandler Actions { get; set; }

        public BaseStoredProcedureBuilder(IDbCommand command, string name)
        {
            Data = new BuilderData(command, name);
            Actions = new ActionsHandler(Data);
        }

        private IDbCommand GetPreparedDbCommand()
        {
            Data.Command.CommandType(DbCommandTypes.StoredProcedure);
            Data.Command.ClearSql.Sql(Data.Command.Data.Context.Data.FluentDataProvider.GetSqlForStoredProcedureBuilder(Data));
            return Data.Command;
        }

        public void Dispose()
        {
            Data.Command.Dispose();
        }

        public DataTable QueryDataTable(params IDataParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                Actions.ParameterAction(parameter);
            }
            return this.QueryDataTable();
        }

        public IBaseStoredProcedureBuilder Parameter(IDataParameter parameter)
        {
            Actions.ParameterAction(parameter);
            return this;
        }

        public object? ParameterValue(string name, bool isFluentType)
        {
            return Data.Command.ParameterValue(name, isFluentType);
        }

        public TParameterType ParameterValue<TParameterType>(string outputParameterName)
        {
            return Data.Command.ParameterValue<TParameterType>(outputParameterName);
        }

        public int Execute()
        {
            return GetPreparedDbCommand().Execute();
        }

        public List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return GetPreparedDbCommand().QueryMany(customMapper);
        }

        public List<TEntity> QueryMany<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return GetPreparedDbCommand().QueryMany(customMapper);
        }

        public TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader>? customMapper = null) where TList : IList<TEntity>
        {
            return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
        }

        public TList QueryMany<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>
        {
            return GetPreparedDbCommand().QueryMany<TEntity, TList>(customMapper);
        }

        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper)
        {
            GetPreparedDbCommand().QueryComplexMany(list, customMapper);
        }

        public void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper)
        {
            GetPreparedDbCommand().QueryComplexMany(list, customMapper);
        }

        public TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader>? customMapper = null)
        {
            return GetPreparedDbCommand().QuerySingle(customMapper);
        }

        public TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper)
        {
            return GetPreparedDbCommand().QuerySingle(customMapper);
        }

        public TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper)
        {
            return GetPreparedDbCommand().QueryComplexSingle(customMapper);
        }

        public TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper)
        {
            return GetPreparedDbCommand().QueryComplexSingle(customMapper);
        }

        public DataTable QueryDataTable()
        {
            return GetPreparedDbCommand().QueryDataTable();
        }
    }
}

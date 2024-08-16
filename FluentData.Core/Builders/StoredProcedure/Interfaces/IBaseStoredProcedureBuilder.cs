using System.Data;

namespace FluentData.Core
{
    public interface IBaseStoredProcedureBuilder : IExecute, IQuery, IParameterValue, IDisposable
    {
        BuilderData Data { get; }

        DataTable QueryDataTable(params IDataParameter[] parameters);

        IBaseStoredProcedureBuilder Parameter(IDataParameter parameter);
    }
}

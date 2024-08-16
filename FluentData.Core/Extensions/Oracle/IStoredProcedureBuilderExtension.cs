using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace FluentData.Core.Extensions.Oracle
{
    public static class IStoredProcedureBuilderExtension
    {
        public static DataSet QueryDataSet(this IBaseStoredProcedureBuilder builder, params IDataParameter[] parameters)
        {
            var builderData = builder.Data;
            using var dbCommand = builderData.Command.Data.InnerCommand;
            dbCommand.CommandText = builderData.ObjectName;
            dbCommand.CommandType = CommandType.StoredProcedure;
            foreach (var parameter in parameters)
            {
                dbCommand.Parameters.Add(parameter);
            }
            using var oracleDataAdapter = new OracleDataAdapter((OracleCommand)dbCommand);
            var dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public static IBaseStoredProcedureBuilder Parameter(this IBaseStoredProcedureBuilder builder, string name, object value, OracleDbType oracleDbType, int size = 0)
        {
            builder.Parameter(new OracleParameter(name, oracleDbType, size)
            {
                Value = value
            });
            return builder;
        }

        public static IBaseStoredProcedureBuilder ParameterOut(this IBaseStoredProcedureBuilder builder, string outputParameterName, OracleDbType oracleDbType, int size = 0)
        {
            builder.Parameter(new OracleParameter(outputParameterName, oracleDbType, size)
            {
                Direction = System.Data.ParameterDirection.Output
            });
            return builder;
        }
    }
}


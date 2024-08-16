using System.Data;
using System.Data.SqlClient;

namespace FluentData.Core.Extensions.SqlServer
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
            using var sqlDataAdapter = new SqlDataAdapter((SqlCommand)dbCommand);
            var dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public static IBaseStoredProcedureBuilder Parameter(this IBaseStoredProcedureBuilder builder, string name, object value, SqlDbType sqlDbType, int size = 0)
        {
            builder.Parameter(new SqlParameter(name, sqlDbType, size)
            {
                Value = value
            });
            return builder;
        }

        public static IBaseStoredProcedureBuilder ParameterOut(this IBaseStoredProcedureBuilder builder, string outputParameterName, SqlDbType sqlDbType, int size = 0)
        {
            builder.Parameter(new SqlParameter(outputParameterName, sqlDbType, size)
            {
                Direction = System.Data.ParameterDirection.Output
            });
            return builder;
        }
    }
}


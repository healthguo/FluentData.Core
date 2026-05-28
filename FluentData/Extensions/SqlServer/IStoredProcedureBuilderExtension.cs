using System.Data;
using System.Data.SqlClient;

namespace FluentData.Extensions.SqlServer
{
    /// <summary>
    /// Provides SQL Server-specific extension methods for stored procedure builders.
    /// Includes support for <see cref="DataSet"/> queries and typed SQL Server parameters.
    /// </summary>
    /// <remarks>
    /// These extensions are only available when referencing the <c>System.Data.SqlClient</c> assembly.
    /// Use them to simplify parameter creation with SQL Server-specific types.
    /// </remarks>
    public static class IStoredProcedureBuilderExtension
    {
        /// <summary>
        /// Executes the stored procedure and returns the result as a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="builder">The stored procedure builder to execute.</param>
        /// <param name="parameters">Optional database parameters to pass to the stored procedure.</param>
        /// <returns>A <see cref="DataSet"/> containing all result sets from the stored procedure.</returns>
        public static DataSet QueryDataSet(this IBaseStoredProcedureBuilder builder, params IDataParameter[] parameters)
        {
            var builderData = builder.Data;
            using (var dbCommand = builderData.Command.Data.InnerCommand)
            {
                dbCommand.CommandText = builderData.ObjectName;
                dbCommand.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                {
                    dbCommand.Parameters.Add(parameter);
                }
                using(var sqlDataAdapter = new SqlDataAdapter((SqlCommand)dbCommand))
                {
                    var dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                    return dataSet;
                }
            }
        }

        /// <summary>
        /// Adds an input parameter with a SQL Server-specific data type to the stored procedure command.
        /// </summary>
        /// <param name="builder">The stored procedure builder to add the parameter to.</param>
        /// <param name="name">The parameter name (with or without the '@' prefix).</param>
        /// <param name="value">The parameter value.</param>
        /// <param name="sqlDbType">The SQL Server data type of the parameter.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IBaseStoredProcedureBuilder"/> instance for method chaining.</returns>
        public static IBaseStoredProcedureBuilder Parameter(this IBaseStoredProcedureBuilder builder, string name, object value, SqlDbType sqlDbType, int size = 0)
        {
            builder.Parameter(new SqlParameter(name, sqlDbType, size)
            {
                Value = value
            });
            return builder;
        }

        /// <summary>
        /// Adds an output parameter with a SQL Server-specific data type to the stored procedure command.
        /// </summary>
        /// <param name="builder">The stored procedure builder to add the parameter to.</param>
        /// <param name="outputParameterName">The output parameter name (with or without the '@' prefix).</param>
        /// <param name="sqlDbType">The SQL Server data type of the output parameter.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IBaseStoredProcedureBuilder"/> instance for method chaining.</returns>
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


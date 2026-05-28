using System.Dynamic;

namespace FluentData
{
    /// <summary>
    /// Dynamic implementation of the stored procedure builder using <see cref="ExpandoObject"/>.
    /// Provides a fluent interface for adding parameters from dynamic objects.
    /// </summary>
    internal class StoredProcedureBuilderDynamic : BaseStoredProcedureBuilder, IStoredProcedureBuilderDynamic
    {
        /// <summary>
        /// Creates a new instance of <see cref="StoredProcedureBuilderDynamic"/>.
        /// </summary>
        /// <param name="command">The database command.</param>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="item">The dynamic object containing parameter values.</param>
        internal StoredProcedureBuilderDynamic(IDbCommand command, string name, ExpandoObject item)
            : base(command, name)
        {
            Data.Item = item;
        }

        /// <summary>
        /// Adds a parameter to the stored procedure.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        public IStoredProcedureBuilderDynamic Parameter(string name, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(name, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Automatically maps all dynamic object properties to stored procedure parameters, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Property names to exclude from mapping.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        public IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties)
        {
            Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
            return this;
        }

        /// <summary>
        /// Adds an output parameter to the stored procedure.
        /// </summary>
        /// <param name="name">The name of the output parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        public IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0)
        {
            Actions.ParameterOutputAction(name, parameterType, size);
            return this;
        }

        /// <summary>
        /// Configures whether to use multiple result sets for the stored procedure.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result sets; otherwise, false.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilderDynamic"/> instance for method chaining.</returns>
        public IStoredProcedureBuilderDynamic UseMultiResult(bool useMultipleResultsets)
        {
            Data.Command.UseMultiResult(useMultipleResultsets);
            return this;
        }
    }
}

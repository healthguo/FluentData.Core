using System.Linq.Expressions;

namespace FluentData.Core
{
    /// <summary>
    /// Strongly-typed implementation of the stored procedure builder.
    /// Supports expression-based parameter mapping and auto-mapping from entity properties.
    /// </summary>
    /// <typeparam name="T">The entity type containing parameter values.</typeparam>
    internal class StoredProcedureBuilder<T> : BaseStoredProcedureBuilder, IStoredProcedureBuilder<T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="StoredProcedureBuilder{T}"/>.
        /// </summary>
        /// <param name="command">The database command.</param>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="item">The entity instance containing parameter values.</param>
        internal StoredProcedureBuilder(IDbCommand command, string name, T item)
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
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder<T> Parameter(string name, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(name, value, parameterType, size);
            return this;
        }

        /// <summary>
        /// Automatically maps all entity properties to stored procedure parameters, excluding specified properties.
        /// </summary>
        /// <param name="ignoreProperties">Lambda expressions specifying properties to exclude from mapping (e.g., x => x.Id).</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
        {
            Actions.AutoMapColumnsAction(ignoreProperties);
            return this;
        }

        /// <summary>
        /// Adds a parameter using a lambda expression to specify the property.
        /// </summary>
        /// <param name="expression">A lambda expression specifying the property (e.g., x => x.Name).</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        /// <summary>
        /// Adds an output parameter to the stored procedure.
        /// </summary>
        /// <param name="name">The name of the output parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0)
        {
            Actions.ParameterOutputAction(name, parameterType, size);
            return this;
        }

        /// <summary>
        /// Configures whether to use multiple result sets for the stored procedure.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result sets; otherwise, false.</param>
        /// <returns>The current <see cref="IStoredProcedureBuilder{T}"/> instance for method chaining.</returns>
        public IStoredProcedureBuilder<T> UseMultiResult(bool useMultipleResultsets)
        {
            Data.Command.UseMultiResult(useMultipleResultsets);
            return this;
        }
    }
}

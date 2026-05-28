using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Base interface for building and executing stored procedure commands.
    /// Encapsulates execution, querying, parameter handling, and disposal semantics for stored procedures.
    /// </summary>
    /// <remarks>
    /// Stored procedure builders may return multiple result sets; use the <see cref="UseMultiResult(bool)"/>
    /// fluent option to enable multi-result handling when supported by the provider.
    /// </remarks>
    public interface IBaseStoredProcedureBuilder : IExecute, IExecuteAsync, IQuery, IQueryAsync, IParameterValue, IDisposable
    {
        /// <summary>
        /// Gets the builder data containing command, parameters, and stored procedure name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Executes the stored procedure and returns the result as a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="parameters">Optional database parameters to pass to the stored procedure.</param>
        /// <returns>A <see cref="DataTable"/> containing the stored procedure result set. Returns an empty <see cref="DataTable"/> when there are no rows.</returns>
        DataTable QueryDataTable(params IDataParameter[] parameters);

        /// <summary>
        /// Adds a pre-configured <see cref="IDataParameter"/> to the stored procedure command.
        /// </summary>
        /// <param name="parameter">The database parameter to add. Cannot be null.</param>
        /// <returns>The current <see cref="IBaseStoredProcedureBuilder"/> instance for method chaining.</returns>
        IBaseStoredProcedureBuilder Parameter(IDataParameter parameter);
    }
}

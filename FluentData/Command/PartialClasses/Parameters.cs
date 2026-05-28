using FluentData.Extensions;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace FluentData
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Adds a parameter to the command with specified name, value, and data type.
        /// Automatically handles list parameters for IN clauses.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="parameterType">The database data type. Defaults to Object.</param>
        /// <param name="direction">The parameter direction. Defaults to Input.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        public IDbCommand Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0)
        {
            if (parameterType != DataTypes.Binary
                && !(value is byte[])
                && ReflectionHelper.IsList(value))
                AddListParameterToInnerCommand(name, value);
            else
                AddParameterToInnerCommand(name, value, parameterType, direction, size);

            return this;
        }

        private int _currentIndex = 0;
        /// <summary>
        /// Adds multiple positional parameters to the command using numeric indices.
        /// </summary>
        /// <param name="parameters">The parameter values to add.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        public IDbCommand Parameters(params object[] parameters)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                Parameter(_currentIndex.ToString(), parameters[_currentIndex]);
                _currentIndex++;
            }
            return this;
        }

        /// <summary>
        /// Converts a list parameter into multiple individual parameters for IN clause support.
        /// </summary>
        /// <param name="name">The original parameter name.</param>
        /// <param name="value">The list of values.</param>
        private void AddListParameterToInnerCommand(string name, object value)
        {
            var list = (IEnumerable)value;

            var newInStatement = new StringBuilder();

            var index = 0;
            foreach (var item in list)
            {
                if (index == 0)
                    newInStatement.Append(" in(");
                else
                    newInStatement.Append(',');

                var parameter = AddParameterToInnerCommand("p" + name + "p" + index.ToString(), item);

                newInStatement.Append(parameter.ParameterName);
                index++;
            }
            newInStatement.Append(')');

            var oldInStatement = string.Format(" in({0})", Data.Context.Data.FluentDataProvider.GetParameterName(name));
            Data.Sql.Replace(oldInStatement, newInStatement.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Adds a single parameter to the underlying ADO.NET command.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="direction">The parameter direction.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The created <see cref="IDbDataParameter"/>.</returns>
        private IDbDataParameter AddParameterToInnerCommand(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0)
        {
            if (value == null)
                value = DBNull.Value;

            if (value.GetType().IsEnum)
                value = (int)value;

            var dbParameter = Data.InnerCommand.CreateParameter();
            if (parameterType == DataTypes.Object)
                dbParameter.DbType = (DbType)Data.Context.Data.FluentDataProvider.GetDbTypeForClrType(value.GetType());
            else
                dbParameter.DbType = (DbType)parameterType;

            dbParameter.ParameterName = Data.Context.Data.FluentDataProvider.GetParameterName(name);
            dbParameter.Direction = (System.Data.ParameterDirection)direction;
            dbParameter.Value = value;
            if (size > 0)
                dbParameter.Size = size;
            Data.InnerCommand.Parameters.Add(dbParameter);

            return dbParameter;
        }

        /// <summary>
        /// Adds an ADO.NET parameter directly to the command.
        /// </summary>
        /// <param name="parameter">The ADO.NET data parameter to add.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the database does not support output parameters.</exception>
        public IDbCommand Parameter(IDataParameter parameter)
        {
            if (!Data.Context.Data.FluentDataProvider.SupportsOutputParameters)
                throw new FluentDataException("The selected database does not support output parameters");
            Data.InnerCommand.Parameters.Add(parameter);
            return this;
        }

        /// <summary>
        /// Adds an output parameter to the command.
        /// </summary>
        /// <param name="name">The name of the output parameter.</param>
        /// <param name="parameterType">The database data type.</param>
        /// <param name="size">The maximum size of the parameter.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the database does not support output parameters.</exception>
        public IDbCommand ParameterOut(string name, DataTypes parameterType, int size)
        {
            if (!Data.Context.Data.FluentDataProvider.SupportsOutputParameters)
                throw new FluentDataException("The selected database does not support output parameters");
            Parameter(name, null, parameterType, ParameterDirection.Output, size);
            return this;
        }

        /// <summary>
        /// Gets the strongly-typed value of an output parameter by name.
        /// </summary>
        /// <typeparam name="TParameterType">The expected type of the parameter value.</typeparam>
        /// <param name="outputParameterName">The name of the output parameter.</param>
        /// <returns>The typed value of the output parameter, or default if DBNull.</returns>
        /// <exception cref="FluentDataException">Thrown if the parameter is not found.</exception>
        public TParameterType ParameterValue<TParameterType>(string outputParameterName)
        {
            outputParameterName = Data.Context.Data.FluentDataProvider.GetParameterName(outputParameterName);
            if (!Data.InnerCommand.Parameters.Contains(outputParameterName))
                throw new FluentDataException(string.Format("Parameter {0} not found", outputParameterName));

            var value = (Data.InnerCommand.Parameters[outputParameterName] as IDataParameter)?.Value;

            if (value == DBNull.Value)
                return default;

            return (TParameterType)value;
        }

        /// <summary>
        /// Gets the value of a parameter by name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="isFluentType">Whether to use FluentData parameter name formatting.</param>
        /// <returns>The value of the parameter, or null if not found.</returns>
        public object ParameterValue(string name, bool isFluentType)
        {
            if (isFluentType)
            {
                name = Data.Context.Data.FluentDataProvider.GetParameterName(name);
            }

            var innerCommand = Data.InnerCommand;
            if (innerCommand.Parameters.Contains(name))
            {
                var parameter = innerCommand.Parameters[name];
                if (parameter != null && parameter is IDataParameter param)
                {
                    return param.Value;
                }
            }

            return null;
        }
    }
}

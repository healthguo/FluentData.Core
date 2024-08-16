using FluentData.Core.Extensions;
using System.Collections;
using System.Data;
using System.Text;

namespace FluentData.Core
{
    internal partial class DbCommand
    {
        public IDbCommand Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0)
        {
            if (parameterType != DataTypes.Binary
                && value is not byte[]
                && ReflectionHelper.IsList(value))
                AddListParameterToInnerCommand(name, value);
            else
                AddParameterToInnerCommand(name, value, parameterType, direction, size);

            return this;
        }

        private int _currentIndex = 0;
        public IDbCommand Parameters(params object[] parameters)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                Parameter(_currentIndex.ToString(), parameters[_currentIndex]);
                _currentIndex++;
            }
            return this;
        }

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

        private IDbDataParameter AddParameterToInnerCommand(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0)
        {
            value ??= DBNull.Value;

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

        public IDbCommand Parameter(IDataParameter parameter)
        {
            if (!Data.Context.Data.FluentDataProvider.SupportsOutputParameters)
                throw new FluentDataException("The selected database does not support output parameters");
            Data.InnerCommand.Parameters.Add(parameter);
            return this;
        }

        public IDbCommand ParameterOut(string name, DataTypes parameterType, int size)
        {
            if (!Data.Context.Data.FluentDataProvider.SupportsOutputParameters)
                throw new FluentDataException("The selected database does not support output parameters");
            Parameter(name, null, parameterType, ParameterDirection.Output, size);
            return this;
        }

        public TParameterType ParameterValue<TParameterType>(string outputParameterName)
        {
            outputParameterName = Data.Context.Data.FluentDataProvider.GetParameterName(outputParameterName);
            if (!Data.InnerCommand.Parameters.Contains(outputParameterName))
                throw new FluentDataException(string.Format("Parameter {0} not found", outputParameterName));

            var value = (Data.InnerCommand.Parameters[outputParameterName] as IDataParameter)?.Value;

            if (value == DBNull.Value)
                return default;

            return (TParameterType)value!;
        }

        public object? ParameterValue(string name, bool isFluentType)
        {
            if (isFluentType)
            {
                name = Data.Context.Data.FluentDataProvider.GetParameterName(name);
            }

            var innerCommand = Data.InnerCommand;
            if (innerCommand.Parameters.Contains(name))
            {
                var parameter = innerCommand.Parameters[name];
                if (parameter != null && parameter.GetType().IsAssignableTo(typeof(IDataParameter)))
                {
                    return ((IDataParameter)parameter).Value;
                }
            }

            return null;
        }
    }
}

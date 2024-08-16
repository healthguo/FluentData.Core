namespace FluentData.Core
{
    internal class DeleteBuilder : BaseDeleteBuilder, IDeleteBuilder
    {
        public DeleteBuilder(IDbCommand command, string tableName)
            : base(command, tableName)
        {
        }

        public IDeleteBuilder Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IDeleteBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }
    }
}

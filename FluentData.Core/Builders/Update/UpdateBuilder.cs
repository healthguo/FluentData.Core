namespace FluentData.Core
{
    internal class UpdateBuilder : BaseUpdateBuilder, IUpdateBuilder, IInsertUpdateBuilder
    {
        internal UpdateBuilder(IDbProvider dbProvider, IDbCommand command, string name)
            : base(dbProvider, command, name)
        {
        }

        public IUpdateBuilder Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.WhereAction(columnName, value, parameterType, size);
            return this;
        }

        public IUpdateBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }

        public IUpdateBuilder Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IUpdateBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        IInsertUpdateBuilder IInsertUpdateBuilder.Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        IInsertUpdateBuilder IInsertUpdateBuilder.ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilder)this).Column(columnName, value, parameterType, size) : this;
        }

        public IUpdateBuilder Fill(Action<IInsertUpdateBuilder> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}

namespace FluentData.Core
{
    internal class InsertBuilder : BaseInsertBuilder, IInsertBuilder, IInsertUpdateBuilder
    {
        internal InsertBuilder(IDbCommand command, string name)
            : base(command, name)
        {
        }

        public IInsertBuilder Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IInsertBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
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

        public IInsertBuilder Fill(Action<IInsertUpdateBuilder> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}

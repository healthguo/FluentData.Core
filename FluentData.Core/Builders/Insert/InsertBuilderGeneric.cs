using System.Linq.Expressions;

namespace FluentData.Core
{
    internal class InsertBuilder<T> : BaseInsertBuilder, IInsertBuilder<T>, IInsertUpdateBuilder<T>
    {
        public T Item { get; private set; }

        internal InsertBuilder(IDbCommand command, string name, T item)
            : base(command, name)
        {
            Data.Item = item;
            Item = item;
        }

        public IInsertBuilder<T> Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IInsertBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        public IInsertBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        public IInsertBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            return condition ? this.Column(expression, parameterType, size) : this;
        }

        public IInsertBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod)
        {
            fillMethod(this);
            return this;
        }

        public IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
        {
            Actions.AutoMapColumnsAction(ignoreProperties);
            return this;
        }

        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilder<T>)this).Column(columnName, value, parameterType, size) : this;
        }

        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilder<T>)this).Column(expression, parameterType, size) : this;
        }
    }
}

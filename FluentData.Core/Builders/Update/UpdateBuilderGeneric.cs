using System.Linq.Expressions;

namespace FluentData.Core
{
    internal class UpdateBuilder<T> : BaseUpdateBuilder, IUpdateBuilder<T>, IInsertUpdateBuilder<T>
    {
        public T Item { get; private set; }

        internal UpdateBuilder(IDbProvider provider, IDbCommand command, string name, T item)
            : base(provider, command, name)
        {
            Data.Item = item;
            Item = item;
        }

        public IUpdateBuilder<T> Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IUpdateBuilder<T> ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        public IUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
        {
            Actions.AutoMapColumnsAction(ignoreProperties);
            return this;
        }

        public IUpdateBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(expression, parameterType, size);
            return this;
        }

        public IUpdateBuilder<T> ColumnIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            return condition ? this.Column(expression, parameterType, size) : this;
        }

        public IUpdateBuilder<T> Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.WhereAction(columnName, value, parameterType, size);
            return this;
        }

        public IUpdateBuilder<T> WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }

        public IUpdateBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType, int size)
        {
            Actions.WhereAction(expression, parameterType, size);
            return this;
        }

        public IUpdateBuilder<T> WhereIf(bool condition, Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(expression, parameterType, size) : this;
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

        public IUpdateBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}

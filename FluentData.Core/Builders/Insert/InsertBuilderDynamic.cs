using System.Dynamic;

namespace FluentData.Core
{
    internal class InsertBuilderDynamic : BaseInsertBuilder, IInsertBuilderDynamic, IInsertUpdateBuilderDynamic
    {
        public dynamic Item { get; private set; }

        internal InsertBuilderDynamic(IDbCommand command, string name, ExpandoObject item)
            : base(command, name)
        {
            Data.Item = item;
            Item = item;
        }

        public IInsertBuilderDynamic Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IInsertBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        public IInsertBuilderDynamic Column(string propertyName, DataTypes parameterType, int size)
        {
            Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
            return this;
        }

        public IInsertBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType, int size)
        {
            return condition ? this.Column(propertyName, parameterType, size) : this;
        }

        public IInsertBuilderDynamic AutoMap(params string[] ignoreProperties)
        {
            Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
            return this;
        }

        public IInsertBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod)
        {
            fillMethod(this);
            return this;
        }

        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilderDynamic)this).Column(columnName, value, parameterType, size) : this;
        }

        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName, DataTypes parameterType, int size)
        {
            Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
            return this;
        }

        IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.ColumnIf(bool condition, string propertyName, DataTypes parameterType, int size)
        {
            return condition ? ((IInsertUpdateBuilderDynamic)this).Column(propertyName, parameterType, size) : this;
        }
    }
}

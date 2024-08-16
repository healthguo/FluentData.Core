using System.Dynamic;

namespace FluentData.Core
{
    internal class UpdateBuilderDynamic : BaseUpdateBuilder, IUpdateBuilderDynamic, IInsertUpdateBuilderDynamic
    {
        public dynamic Item { get; private set; }

        internal UpdateBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
            : base(dbProvider, command, name)
        {
            Data.Item = item;
            Item = item;
        }

        public IUpdateBuilderDynamic Where(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.WhereAction(columnName, value, parameterType, size);
            return this;
        }

        public IUpdateBuilderDynamic WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(columnName, value, parameterType, size) : this;
        }

        public IUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType, int size)
        {
            Actions.ColumnValueAction(columnName, value, parameterType, size);
            return this;
        }

        public IUpdateBuilderDynamic ColumnIf(bool condition, string columnName, object value, DataTypes parameterType, int size)
        {
            return condition ? this.Column(columnName, value, parameterType, size) : this;
        }

        public IUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType, int size)
        {
            Actions.ColumnValueDynamic((ExpandoObject)Data.Item, propertyName, parameterType, size);
            return this;
        }

        public IUpdateBuilderDynamic ColumnIf(bool condition, string propertyName, DataTypes parameterType, int size)
        {
            return condition ? this.Column(propertyName, parameterType, size) : this;
        }

        public IUpdateBuilderDynamic Where(string name, DataTypes parameterType, int size)
        {
            var propertyValue = ReflectionHelper.GetPropertyValueDynamic(Data.Item, name);
            Where(name, propertyValue, parameterType, size);
            return this;
        }

        public IUpdateBuilderDynamic WhereIf(bool condition, string name, DataTypes parameterType = DataTypes.Object, int size = 0)
        {
            return condition ? this.Where(name, parameterType, size) : this;
        }

        public IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties)
        {
            Actions.AutoMapDynamicTypeColumnsAction(ignoreProperties);
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

        public IUpdateBuilderDynamic Fill(Action<IInsertUpdateBuilderDynamic> fillMethod)
        {
            fillMethod(this);
            return this;
        }
    }
}

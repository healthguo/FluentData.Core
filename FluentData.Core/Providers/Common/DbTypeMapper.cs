using System.Xml.Linq;

namespace FluentData.Core.Providers.Common
{
    internal class DbTypeMapper
    {
        private static Dictionary<Type, DataTypes>? _types;

        private static readonly object _locker = new();

        public DataTypes GetDbTypeForClrType(Type clrType)
        {
            if (_types == null)
            {
                lock (_locker)
                {
                    _types = new Dictionary<Type, DataTypes>
                    {
                        { typeof(short), DataTypes.Int16 },
                        { typeof(int), DataTypes.Int32 },
                        { typeof(long), DataTypes.Int64 },
                        { typeof(string), DataTypes.String },
                        { typeof(DateTime), DataTypes.DateTime },
                        { typeof(XDocument), DataTypes.Xml },
                        { typeof(decimal), DataTypes.Decimal },
                        { typeof(Guid), DataTypes.Guid },
                        { typeof(bool), DataTypes.Boolean },
                        { typeof(char), DataTypes.String },
                        { typeof(DBNull), DataTypes.String },
                        { typeof(float), DataTypes.Single },
                        { typeof(double), DataTypes.Double },
                        { typeof(byte[]), DataTypes.Binary }
                    };
                }
            }

            if (!_types.ContainsKey(clrType))
                return DataTypes.Object;

            return _types[clrType];
        }
    }
}

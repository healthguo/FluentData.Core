using System.Reflection;

namespace FluentData.Core
{
    /// <summary>
    /// Automatically maps data reader fields to entity properties.
    /// Supports simple property mapping, nested object mapping, and enum conversion.
    /// </summary>
    internal class AutoMapper
    {
        private readonly DbCommandData _dbCommandData;

        private readonly Dictionary<string, PropertyInfo> _properties;

        private readonly List<DataReaderField> _fields;

        private readonly System.Data.IDataReader _reader;

        /// <summary>
        /// Creates a new instance of <see cref="AutoMapper"/>.
        /// </summary>
        /// <param name="dbCommandData">The command data containing the data reader.</param>
        /// <param name="itemType">The type of entity to map to.</param>
        internal AutoMapper(DbCommandData dbCommandData, Type itemType)
        {
            _dbCommandData = dbCommandData;
            _reader = dbCommandData.Reader.InnerReader;
            _properties = ReflectionHelper.GetProperties(itemType);
            _fields = DataReaderHelper.GetDataReaderFields(_reader);
        }

        /// <summary>
        /// Automatically maps all data reader fields to the specified entity object.
        /// </summary>
        /// <param name="item">The entity object to populate with data reader values.</param>
        /// <exception cref="FluentDataException">Thrown if a field cannot be mapped and IgnoreIfAutoMapFails is false.</exception>
        public void AutoMap(object item)
        {
            foreach (var field in _fields)
            {
                if (field.IsSystem)
                    continue;

                var value = _reader.GetValue(field.Index);
                var wasMapped = false;


                if (_properties.TryGetValue(field.LowerName, out PropertyInfo? property))
                {
                    SetPropertyValue(field, property, item, value);
                    wasMapped = true;
                }
                else
                {
                    if (field.LowerName.IndexOf('_') != -1)
                        wasMapped = HandleComplexField(item, field, value);
                }

                if (!wasMapped && !_dbCommandData.Context.Data.IgnoreIfAutoMapFails)
                    throw new FluentDataException("Could not map: " + field.Name);
            }
        }

        /// <summary>
        /// Handles complex field mapping for nested properties using underscore notation.
        /// </summary>
        /// <param name="item">The parent entity object.</param>
        /// <param name="field">The data reader field information.</param>
        /// <param name="value">The value to map.</param>
        /// <returns>True if the field was successfully mapped; otherwise, false.</returns>
        private bool HandleComplexField(object item, DataReaderField field, object? value)
        {
            string? propertyName = null;

            for (var level = 0; level <= field.NestedLevels; level++)
            {
                if (string.IsNullOrEmpty(propertyName))
                    propertyName = field.GetNestedName(level);
                else
                    propertyName += "_" + field.GetNestedName(level);

                var properties = ReflectionHelper.GetProperties(item.GetType());
                if (properties.TryGetValue(propertyName, out PropertyInfo? property))
                {
                    if (level == field.NestedLevels)
                    {
                        SetPropertyValue(field, property, item, value);
                        return true;
                    }
                    else
                    {
                        item = GetOrCreateInstance(item, property);
                        if (item == null)
                            return false;
                        propertyName = null;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets an existing property value or creates a new instance if null.
        /// </summary>
        /// <param name="item">The parent object containing the property.</param>
        /// <param name="property">The property to get or create.</param>
        /// <returns>The existing or newly created instance.</returns>
        private object GetOrCreateInstance(object item, PropertyInfo property)
        {
            var instance = ReflectionHelper.GetPropertyValue(item, property);

            if (instance == null)
            {
                instance = _dbCommandData.Context.Data.EntityFactory.Create(property.PropertyType);

                property.SetValue(item, instance, null);
            }

            return instance;
        }

        /// <summary>
        /// Sets the property value on the entity, handling type conversions and null values.
        /// </summary>
        /// <param name="field">The data reader field information.</param>
        /// <param name="property">The property to set.</param>
        /// <param name="item">The entity object.</param>
        /// <param name="value">The value to set.</param>
        /// <exception cref="FluentDataException">Thrown if the property cannot be set.</exception>
        private void SetPropertyValue(DataReaderField field, PropertyInfo property, object item, object? value)
        {
            try
            {
                if (value == DBNull.Value)
                {
                    if (ReflectionHelper.IsNullable(property))
                        value = null;
                    else
                        value = ReflectionHelper.GetDefault(property.PropertyType);
                }
                else
                {
                    var propertyType = ReflectionHelper.GetPropertyType(property);

                    if (propertyType != field.Type)
                    {
                        if (propertyType.IsEnum)
                        {
                            if (field.Type == typeof(string))
                                value = Enum.Parse(propertyType, value?.ToString(), true);
                            else
                                value = Enum.ToObject(propertyType, value);
                        }
                        else if (!ReflectionHelper.IsBasicClrType(propertyType))
                            return;
                        else if (propertyType == typeof(string))
                            value = value?.ToString();
                        else
                            value = Convert.ChangeType(value, property.PropertyType);
                    }
                }

                property.SetValue(item, value, null);
            }
            catch (Exception exception)
            {
                throw new FluentDataException("Could not map: " + property.Name, exception);
            }
        }
    }
}

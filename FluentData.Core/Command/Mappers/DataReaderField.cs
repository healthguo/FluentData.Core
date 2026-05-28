namespace FluentData.Core
{
    /// <summary>
    /// Represents metadata about a data reader field including name, type, and nested property information.
    /// </summary>
    internal class DataReaderField
    {
        /// <summary>
        /// Gets the zero-based index of the field in the data reader.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the lowercase name of the field for case-insensitive comparisons.
        /// </summary>
        public string LowerName { get; private set; }

        /// <summary>
        /// Gets the original name of the field.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the CLR type of the field.
        /// </summary>
        public Type Type { get; private set; }

        private readonly string[] _nestedPropertyNames;

        private readonly int _nestedLevels;

        /// <summary>
        /// Creates a new instance of <see cref="DataReaderField"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="type">The CLR type of the field.</param>
        public DataReaderField(int index, string name, Type type)
        {
            Index = index;
            Name = name;
            LowerName = name.ToLower();
            Type = type;
            _nestedPropertyNames = LowerName.Split('_');
            _nestedLevels = _nestedPropertyNames.Length - 1;
        }

        /// <summary>
        /// Gets the nested property name at the specified level.
        /// </summary>
        /// <param name="level">The nesting level (zero-based).</param>
        /// <returns>The property name at the specified level.</returns>
        public string GetNestedName(int level)
        {
            return _nestedPropertyNames[level];
        }

        /// <summary>
        /// Gets the number of nested property levels (based on underscores in the field name).
        /// </summary>
        public int NestedLevels
        {
            get
            {
                return _nestedLevels;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is a system field (used internally by FluentData).
        /// </summary>
        public bool IsSystem
        {
            get
            {
                return Name.IndexOf("FLUENTDATA_") > -1;
            }
        }
    }
}

using System;
using System.Dynamic;

namespace FluentData
{
    /// <summary>
    /// Provides dynamic access to IDataReader columns using property-like syntax.
    /// </summary>
    internal class DynamicDataReader : DynamicObject
    {
        private readonly System.Data.IDataReader _dataReader;

        /// <summary>
        /// Creates a new instance of <see cref="DynamicDataReader"/>.
        /// </summary>
        /// <param name="dataReader">The IDataReader to wrap.</param>
        internal DynamicDataReader(System.Data.IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        /// <summary>
        /// Provides the implementation for getting member values dynamically.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation.</param>
        /// <param name="result">The result of the get operation. DBNull values are converted to null.</param>
        /// <returns>Always returns true to indicate the operation was handled.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dataReader[binder.Name];
            if (result == DBNull.Value)
                result = null;

            return true;
        }
    }
}

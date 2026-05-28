using System;

namespace FluentData
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Executes the INSERT command and returns the last inserted identity value.
        /// </summary>
        /// <typeparam name="T">The type of the identity value to return.</typeparam>
        /// <param name="identityColumnName">The name of the identity column. If null, the provider's default identity column is used.</param>
        /// <returns>The last inserted identity value.</returns>
        /// <exception cref="FluentDataException">Thrown if the identity column is required but not provided.</exception>
        public T ExecuteReturnLastId<T>(string identityColumnName = null)
        {
            if (Data.Context.Data.FluentDataProvider.RequiresIdentityColumn && string.IsNullOrEmpty(identityColumnName))
                throw new FluentDataException("The identity column must be given");

            var value = Data.Context.Data.FluentDataProvider.ExecuteReturnLastId<T>(this, identityColumnName);
            T lastId;

            if (value.GetType() == typeof(T))
                lastId = (T)value;
            else
                lastId = (T)Convert.ChangeType(value, typeof(T));

            return lastId;
        }
    }
}

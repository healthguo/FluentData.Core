using System;
using System.Data;

namespace FluentData
{
    /// <summary>
    /// Provides data for connection events (opening, opened, closed).
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="IDbConnection"/> associated with the event.
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ConnectionEventArgs"/>.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> associated with the event.</param>
        public ConnectionEventArgs(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}

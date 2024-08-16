using System.Data;

namespace FluentData.Core
{
    public class ConnectionEventArgs : EventArgs
    {
        public IDbConnection Connection { get; private set; }

        public ConnectionEventArgs(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}

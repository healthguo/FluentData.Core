﻿namespace FluentData.Core
{
    public partial class DbContext : IDbContext
    {
        public DbContextData Data { get; private set; }

        public DbContext()
        {
            Data = new DbContextData();
        }

        internal void CloseSharedConnection()
        {
            if (Data.Connection == null)
                return;

            if (Data.UseTransaction
                && Data.Transaction != null)
                Rollback();

            Data.Connection.Close();

            Data.OnConnectionClosed?.Invoke(new ConnectionEventArgs(Data.Connection));
        }

        public void Dispose()
        {
            CloseSharedConnection();
        }
    }
}

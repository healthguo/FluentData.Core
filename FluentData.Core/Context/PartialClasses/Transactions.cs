namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Enables or disables transaction support for the context.
        /// </summary>
        /// <param name="useTransaction">True to enable transaction support; otherwise, false.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext UseTransaction(bool useTransaction)
        {
            Data.UseTransaction = useTransaction;
            return this;
        }

        /// <summary>
        /// Enables or disables shared connection usage across commands.
        /// </summary>
        /// <param name="useSharedConnection">True to share a single connection across commands; otherwise, false.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext UseSharedConnection(bool useSharedConnection)
        {
            Data.UseSharedConnection = useSharedConnection;
            return this;
        }

        /// <summary>
        /// Sets the isolation level for transactions.
        /// </summary>
        /// <param name="isolationLevel">The <see cref="IsolationLevel"/> to use for transactions.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext IsolationLevel(IsolationLevel isolationLevel)
        {
            Data.IsolationLevel = isolationLevel;
            return this;
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if transaction support has not been enabled.</exception>
        public IDbContext Commit()
        {
            TransactionAction(() => Data.Transaction.Commit());
            return this;
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if transaction support has not been enabled.</exception>
        public IDbContext Rollback()
        {
            TransactionAction(() => Data.Transaction.Rollback());
            return this;
        }

        /// <summary>
        /// Executes an action within a transaction context, validating transaction state.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="FluentDataException">Thrown if transaction support has not been enabled.</exception>
        private void TransactionAction(Action action)
        {
            if (Data.Transaction == null)
                return;
            if (!Data.UseTransaction)
                throw new FluentDataException("Transaction support has not been enabled.");
            action();
            Data.Transaction = null;
        }
    }
}

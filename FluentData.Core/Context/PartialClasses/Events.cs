namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Registers an action to be invoked before a connection opens.
        /// </summary>
        /// <param name="action">The action to invoke with connection event arguments.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnConnectionOpening(Action<ConnectionEventArgs> action)
        {
            Data.OnConnectionOpening = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked after a connection opens.
        /// </summary>
        /// <param name="action">The action to invoke with connection event arguments.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnConnectionOpened(Action<ConnectionEventArgs> action)
        {
            Data.OnConnectionOpened = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked after a connection closes.
        /// </summary>
        /// <param name="action">The action to invoke with connection event arguments.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnConnectionClosed(Action<ConnectionEventArgs> action)
        {
            Data.OnConnectionClosed = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked before a command executes.
        /// </summary>
        /// <param name="action">The action to invoke with command event arguments.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnExecuting(Action<CommandEventArgs> action)
        {
            Data.OnExecuting = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked after a command executes.
        /// </summary>
        /// <param name="action">The action to invoke with command event arguments.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnExecuted(Action<CommandEventArgs> action)
        {
            Data.OnExecuted = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked when an error occurs during command execution.
        /// </summary>
        /// <param name="action">The action to invoke with error event arguments.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnError(Action<ErrorEventArgs> action)
        {
            Data.OnError = action;
            return this;
        }
    }
}

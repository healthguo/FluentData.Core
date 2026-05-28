using System;

namespace FluentData
{
    public partial class DbContext
    {
        /// <summary>
        /// Registers an action to be invoked before a connection is opened.
        /// </summary>
        /// <param name="action">The action to execute, receiving <see cref="ConnectionEventArgs"/>.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnConnectionOpening(Action<ConnectionEventArgs> action)
        {
            Data.OnConnectionOpening = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked after a connection is opened.
        /// </summary>
        /// <param name="action">The action to execute, receiving <see cref="ConnectionEventArgs"/>.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnConnectionOpened(Action<ConnectionEventArgs> action)
        {
            Data.OnConnectionOpened = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked when a connection is closed.
        /// </summary>
        /// <param name="action">The action to execute, receiving <see cref="ConnectionEventArgs"/>.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnConnectionClosed(Action<ConnectionEventArgs> action)
        {
            Data.OnConnectionClosed = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked before a command is executed.
        /// </summary>
        /// <param name="action">The action to execute, receiving <see cref="CommandEventArgs"/>.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnExecuting(Action<CommandEventArgs> action)
        {
            Data.OnExecuting = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked after a command has been executed.
        /// </summary>
        /// <param name="action">The action to execute, receiving <see cref="CommandEventArgs"/>.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnExecuted(Action<CommandEventArgs> action)
        {
            Data.OnExecuted = action;
            return this;
        }

        /// <summary>
        /// Registers an action to be invoked when an error occurs during command execution.
        /// </summary>
        /// <param name="action">The action to execute, receiving <see cref="ErrorEventArgs"/>.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        public IDbContext OnError(Action<ErrorEventArgs> action)
        {
            Data.OnError = action;
            return this;
        }
    }
}

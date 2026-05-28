namespace FluentData.Core
{
    /// <summary>
    /// Provides data for error events that occur during command execution.
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="System.Data.IDbCommand"/> that caused the error.
        /// </summary>
        public System.Data.IDbCommand Command { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="Exception"/> that occurred.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ErrorEventArgs"/>.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> that caused the error.</param>
        /// <param name="exception">The <see cref="Exception"/> that occurred.</param>
        public ErrorEventArgs(System.Data.IDbCommand command, Exception exception)
        {
            Command = command;
            Exception = exception;
        }
    }
}

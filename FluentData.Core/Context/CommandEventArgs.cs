namespace FluentData.Core
{
    /// <summary>
    /// Provides data for command execution events.
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="System.Data.IDbCommand"/> being executed.
        /// </summary>
        public System.Data.IDbCommand Command { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="CommandEventArgs"/>.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> being executed.</param>
        public CommandEventArgs(System.Data.IDbCommand command)
        {
            Command = command;
        }
    }
}

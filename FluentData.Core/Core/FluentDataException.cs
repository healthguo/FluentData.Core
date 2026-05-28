namespace FluentData.Core
{
    /// <summary>
    /// Represents errors that occur during FluentData operations.
    /// </summary>
    public class FluentDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FluentDataException"/> with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FluentDataException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FluentDataException"/> with a specified error message and a reference to the inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public FluentDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

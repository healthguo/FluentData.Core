using System.Threading.Tasks;

namespace FluentData
{
    internal partial class DbCommand
    {
        /// <summary>
        /// Executes the SQL command and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected by the command.</returns>
        public int Execute()
        {
            var recordsAffected = 0;

            Data.ExecuteQueryHandler.ExecuteQuery(false, () =>
            {
                recordsAffected = Data.InnerCommand.ExecuteNonQuery();
            });
            return recordsAffected;
        }

        /// <summary>
        /// Executes the SQL command asynchronously and returns the number of rows affected.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning the number of rows affected.</returns>
        public Task<int> ExecuteAsync()
        {
            return Task.FromResult(Execute());
        }
    }
}

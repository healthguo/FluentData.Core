using FluentData.Core;

namespace FluentData.Test.UnitTests
{
    public class DbHelper
    {
        private static IDbContext? context = null;
        public static IDbContext Context
        {
            get
            {
                if (context == null)
                {
                    var connectionString = "Data Source=|DataDirectory|\\test.db";
                    context = new DbContext().ConnectionString(connectionString, new SqliteProvider());
                }
                return context;
            }
        }

    }
}
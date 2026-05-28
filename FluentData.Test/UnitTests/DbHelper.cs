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
                context ??= new DbContext().ConnectionStringNameFromConfigFile("connectionString", new SqliteProvider());
                return context;
            }
        }

    }
}
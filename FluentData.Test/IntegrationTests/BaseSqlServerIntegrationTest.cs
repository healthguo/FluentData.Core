using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests
{
    public abstract class BaseSqlServerIntegrationTest
    {
        public IDbContext Context
        {
            get { return TestHelper.Context(); }
        }
    }
}

using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class EntityFactoryTests : BaseSqlServerIntegrationTest
    {
        public void Test()
        {
            Context.EntityFactory(new CustomEntityFactory())
                .Sql("select * from Product")
                .QueryMany<Product>();
        }

        public class CustomEntityFactory : IEntityFactory
        {
            public virtual object Create(Type type)
            {
                return Activator.CreateInstance(type);
            }
        }
    }
}

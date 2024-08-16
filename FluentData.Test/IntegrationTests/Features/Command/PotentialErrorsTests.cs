using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Command
{

    public class PotentialErrorsTests : BaseSqlServerIntegrationTest
    {

        public void AutoMap_same_name_but_different_type()
        {
            Context.Sql("select 1 as Name, 'AB' as CategoryId from Category where CategoryId = 1")
                .QuerySingle<Category>();
        }
    }
}

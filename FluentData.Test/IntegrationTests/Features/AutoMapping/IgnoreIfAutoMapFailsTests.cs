using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.AutoMapping
{

	public class IgnoreIfAutoMapFailsTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_different_columns_and_properties_automap_must_fail()
        {
			Context.Sql(@"select CategoryId as CategoryIdNotExist, Name from Category")
				.QueryMany<Category>();
        }

		
		public void Test_ignoreIfAutoMapFails_different_columns_and_properties_automap_must_not_fail()
		{
			Context.IgnoreIfAutoMapFails(true)
				.Sql(@"select CategoryId as CategoryIdNotExist, Name from Category")
				.QueryMany<Category>();
		}

		
		public void Test_same_columns_and_properties_automap_must_not_fail()
		{
			Context.Sql(@"select CategoryId as CategoryIdNotExist, Name from Category")
				.QueryMany<Category>();
		}
	}
}

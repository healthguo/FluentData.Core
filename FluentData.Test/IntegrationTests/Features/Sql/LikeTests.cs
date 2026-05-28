using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Sql
{

	public class LikeTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			Context.Sql("select * from Product where Name like @Name")
				.Parameter("Name", "The %")
				.QueryMany<Product>();
		}
	}
}
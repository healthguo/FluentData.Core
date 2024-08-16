using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
	public class SelectDataTests : BaseSqlServerIntegrationTest
	{
		public void Test()
		{
			var count = Context.Sql("select count(*) from Product")
							.QuerySingle<int>();
			Context.Select<Product>("p.*, c.Name as Category_Name")
			    .From(@"Product p inner join Category c on c.CategoryId = p.CategoryId")
			    .Where("p.ProductId > 0 and p.Name is not null")
			    .OrderBy("p.Name")
			    .Paging(1, 10)
				.QueryMany();
		}
	}
}

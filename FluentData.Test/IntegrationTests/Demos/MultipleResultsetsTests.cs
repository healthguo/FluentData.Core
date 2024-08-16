using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class MultipleResultsetsTests : BaseSqlServerIntegrationTest
	{
		public void MultipleResultset()
		{
			using (var command = Context.MultiResultSql)
			{
				command.Sql(@"select * from Category;select * from Product;")
					.QueryMany<Category>();

				command.QueryMany<Product>();

			}
		}
	}
}

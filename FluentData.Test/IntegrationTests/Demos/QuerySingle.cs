using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
	public class QuerySingle : BaseSqlServerIntegrationTest
	{
		public void Query_single_dynamic()
		{
			Context.Sql("select * from Product where ProductId = 1")
				.QuerySingle<dynamic>();
		}

		public void Query_single_strongly_typed()
		{
			Context.Sql("select * from Product where ProductId = 1")
				.QuerySingle<Product>();
		}
	}
}

using System.Data;

namespace FluentData.Test.IntegrationTests.Features.Queries
{

	public class QueryDataTableTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_QueryMany()
		{
			Context.Sql("select top 3 * from Product")
				.QueryMany<DataTable>();
		}

		
		public void Test_QuerySingle()
		{
			Context.Sql("select top 3 * from Product")
				.QuerySingle<DataTable>();
		}
	}
}

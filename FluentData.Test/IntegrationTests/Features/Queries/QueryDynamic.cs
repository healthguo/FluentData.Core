namespace FluentData.Test.IntegrationTests.Features.Queries
{

	public class QueryDynamic : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			Context.Sql("select * from Category")
				.QueryMany<dynamic>();
		}
	}
}

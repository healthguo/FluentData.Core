namespace FluentData.Test.IntegrationTests.Demos
{
    public class QueryValueTests : BaseSqlServerIntegrationTest
	{
		public void Test()
		{
			Context.Sql(@"select count(*) from Product")
				.QuerySingle<int>();
		}
	}
}

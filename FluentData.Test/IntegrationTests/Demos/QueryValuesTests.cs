namespace FluentData.Test.IntegrationTests.Demos
{
    public class QueryValuesTests : BaseSqlServerIntegrationTest
	{
		public void Test()
		{
			Context.Sql(@"select ProductId from Product")
				.QueryMany<int>();
		}
	}
}

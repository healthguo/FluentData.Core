namespace FluentData.Test.IntegrationTests.Features.Events
{

	public class OnExecutedTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			Context.Sql("select top 1 * from product").QueryMany<dynamic>();
		}
	}
}

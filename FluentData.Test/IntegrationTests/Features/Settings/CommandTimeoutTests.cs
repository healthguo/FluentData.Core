namespace FluentData.Test.IntegrationTests.Features.Settings
{

	public class CommandTimeoutTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			Context.CommandTimeout(330).Sql("select top 1 * from product").QueryMany<dynamic>();
		}
	}
}

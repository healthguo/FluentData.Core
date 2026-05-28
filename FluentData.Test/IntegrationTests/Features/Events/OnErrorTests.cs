namespace FluentData.Test.IntegrationTests.Features.Events
{

	public class OnErrorTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
        {
            Context.Sql("sql with error").QueryMany<dynamic>();
        }
	}
}

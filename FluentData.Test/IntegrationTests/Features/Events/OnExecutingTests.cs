namespace FluentData.Test.IntegrationTests.Features.Events
{

	public class OnExecutingTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			Context.OnExecuting(args =>
				{
					args.Command.CommandText = "select top 1 * from Product";
				})
				.Sql("sql with error").QueryMany<dynamic>();
		}
	}
}

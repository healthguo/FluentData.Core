namespace FluentData.Test.IntegrationTests.Features.Command
{

	public class MultipleResultsetTests : BaseSqlServerIntegrationTest
	{
		
		public void Command_with_multiple_resultset()
		{
			using (var cmd = Context.MultiResultSql)
			{
				cmd.Sql(@"select * from Category where CategoryId = 1;
						select * from Category where CategoryId = 2;")
					.QuerySingle<dynamic>();

				cmd.QuerySingle<dynamic>();
			}
		}
	}
}

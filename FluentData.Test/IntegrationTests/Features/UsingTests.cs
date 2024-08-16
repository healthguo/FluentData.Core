using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features
{
	public class UsingTests : BaseSqlServerIntegrationTest
	{
		public void Context_uneeded_using_statement_must_not_throw_an_exception()
		{
			using (var context = Context)
			{
				context.Sql(@"select * from Category")
					.QueryMany<Category>();
			}
		}

		public void Command_uneeded_using_statement_must_not_throw_an_exception()
		{
			using (var command = Context.Sql(@"select * from Category"))
			{
				command.QueryMany<Category>();
			}
		}
	}
}

﻿namespace FluentData.Test.IntegrationTests.Features.Transaction
{

	public class TransactionTests : BaseSqlServerIntegrationTest
	{
		
		public void Continue_after_rollback_or_commit()
		{
			using (var context = Context.UseTransaction(true))
			{
				var category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Commit();

				category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();
			}
		}

		
		public void Multiple_commits_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				var category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Commit();

				category = context.Sql("select top 1 * from category").QuerySingle<dynamic>();

				context.Commit();
			}
		}

		
		public void Multiple_commits_without_command_executed_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Commit();
				context.Commit();
			}
		}

		
		public void Rollback_when_no_command_executed_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Rollback();
			}
		}

		
		public void Multiple_rollbacks_without_command_executed_should_not_throw_exception()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Rollback();
				context.Rollback();
			}
		}
	}
}
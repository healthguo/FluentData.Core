namespace FluentData.Test.IntegrationTests.Demos
{
	public class TransactionsTests : BaseSqlServerIntegrationTest
	{
		public void Test_transactions()
		{
			using (var db = Context.UseTransaction(true))
			{
				db.Sql("update Product set Name = @0 where ProductId = @1", "The Warren Buffet Way", 1)
					.Execute();

				db.Sql("update Product set Name = @0 where ProductId = @1", "Bill Gates Bio", 2)
					.Execute();

				db.Commit();
			}
		}
	}
}

namespace FluentData.Test.IntegrationTests.Features.Queries
{

	public class QueryValuesTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_int()
		{
			Context.Sql("select CategoryId from Category order by CategoryId")
				.QueryMany<int>();
		}

		
		public void Test_string()
		{
			Context.Sql("select Name from Category order by Name")
				.QueryMany<string>();
		}

		
		public void Test_null()
		{
			Context.Sql("select null")
				.QuerySingle<int>();
		}

		
		public void Test_datatypes()
		{
			Context.Sql("select getdate()")
				.QuerySingle<DateTime>();
		}
	}
}

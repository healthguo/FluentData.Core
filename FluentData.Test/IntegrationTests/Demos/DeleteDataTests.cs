namespace FluentData.Test.IntegrationTests.Demos
{
	public class DeleteDataTests : BaseSqlServerIntegrationTest
	{
		public void Delete_data_sql()
		{
			var productId = Context.Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
								.ExecuteReturnLastId<int>();

			Context.Sql("delete from Product where ProductId = @0", productId)
				.Execute();

		}

		public void Delete_data_builder()
		{
			var productId = Context.Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
								.ExecuteReturnLastId<int>();

			Context.Delete("Product")
				.Where("ProductId", productId)
				.Execute();

		}
	}
}

using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Delete
{

	public class DeleteBuilderTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			using (var db = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(db, "Test", 1);

				var product = TestHelper.GetProduct(db, productId);

				db.Delete("Product")
					.Where("ProductId", productId)
					.Where("Name", "Test")
					.Execute();

				product = TestHelper.GetProduct(db, productId);
			}
		}
	}
}

using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Delete
{

	public class DeleteBuilderGenericTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			using (var db = Context.UseTransaction(true))
			{
				var productId = db.Insert("Product")
									.Column("Name", "Test")
									.Column("CategoryId", 1)
									.ExecuteReturnLastId<int>();

				var product = TestHelper.GetProduct(db, productId);

				db.Delete("Product", product)
					.Where("ProductId", product.ProductId)
					.Where(x => x.Name)
					.Execute();

				product = TestHelper.GetProduct(db, productId);
			}
		}
	}
}

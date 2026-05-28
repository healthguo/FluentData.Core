using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Insert
{

	public class InsertBuilderGenericTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_No_Automap()
		{
			var product = new Product();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = Context.UseTransaction(true))
			{
				product.ProductId = context.Insert<Product>("Product", product)
										.Column("Name", "TestProduct")
										.Column(x => x.CategoryId)
										.ExecuteReturnLastId<int>();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
			}
		}

		
		public void TestAutomap()
		{
			var product = new Product();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = Context.UseTransaction(true))
			{
				product.ProductId = context.Insert("Product", product)
										.AutoMap(x => x.ProductId, x => x.Category)
										.ExecuteReturnLastId<int>();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
			}
		}
	}
}

using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Update
{

	public class UpdateBuilderGenericTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_No_Automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				var product = TestHelper.GetProduct(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				context.Update("Product", product)
					.Column(x => x.Name)
					.Column(x => x.CategoryId)
					.Where(x => x.ProductId)
					.Execute();

				product = TestHelper.GetProduct(context, productId);
			}
		}

		
		public void Test_Automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				var product = TestHelper.GetProduct(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				context.Update("Product", product)
					.AutoMap(x => x.ProductId, x => x.Category)
					.Where(x => x.ProductId)
					.Execute();

				product = TestHelper.GetProduct(context, productId);
			}
		}
	}
}

using FluentData.Test.IntegrationTests.Models;
using System.Dynamic;

namespace FluentData.Test.IntegrationTests.Features.Builders.Update
{

	public class UpdateBuilderDynamicTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_No_Automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				dynamic product = TestHelper.GetProductDynamic(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				context.Update("Product", (ExpandoObject) product)
					.Column("Name", (string) product.Name)
					.Column("CategoryId")
					.Where("ProductId")
					.Execute();

				product = TestHelper.GetProductDynamic(context, productId);
			}
		}

		
		public void Test_Automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				dynamic product = TestHelper.GetProductDynamic(context, productId);
				product.Name = "NewTestProduct";
				product.CategoryId = 2;

				context.Update("Product", (ExpandoObject)product)
					.AutoMap("ProductId")
					.Where("ProductId")
					.Execute();

				product = TestHelper.GetProduct(context, productId);
			}
		}
	}
}

using FluentData.Test.IntegrationTests.Models;
using System.Dynamic;

namespace FluentData.Test.IntegrationTests.Features.Builders.Insert
{
	public class InsertBuilderDynamicTests : BaseSqlServerIntegrationTest
	{		
		public void Test_No_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = Context.UseTransaction(true))
			{
				product.ProductId = context.Insert("Product", (ExpandoObject) product)
									.Column("Name", (string)product.Name)
									.Column("CategoryId", (int)product.CategoryId)
									.ExecuteReturnLastId<int>();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
			}
		}

		
		public void Test_Automap()
		{
			dynamic product = new ExpandoObject();
			product.CategoryId = 1;
			product.Name = "TestProduct";

			using (var context = Context.UseTransaction(true))
			{
				product.ProductId = context.Insert("Product", (ExpandoObject)product)
									.AutoMap("ProductId")
									.ExecuteReturnLastId<int>();

				var createdProduct = TestHelper.GetProduct(context, product.ProductId);
			}
		}
	}
}

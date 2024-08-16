using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Update
{

	public class UpdateBuilderTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = TestHelper.InsertProduct(context, "OldTestProduct", 1);

				context.Update("Product")
					.Column("Name", "NewTestProduct")
					.Column("CategoryId", 2)
					.Where("ProductId", productId)
					.Execute();
								
				var product = TestHelper.GetProduct(context, productId);				
			}
		}
	}
}

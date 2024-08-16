using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Insert
{

	public class InsertBuilderTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			using (var context = Context.UseTransaction(true))
			{
				var productId = context.Insert("Product")
									.Column("Name", "TestProduct")
									.Column("CategoryId", 1)
									.ExecuteReturnLastId<int>();

				var product = TestHelper.GetProduct(context, productId);
			}
		}
	}
}

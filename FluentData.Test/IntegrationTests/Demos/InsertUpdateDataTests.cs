using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
	public class InsertUpdateDataTests : BaseSqlServerIntegrationTest
	{
		public void Test()
		{
            var product = new Product
            {
                Name = "The Warren Buffet Way",
                CategoryId = 1
            };

            product.ProductId = Context.Insert("Product", product)
									.Fill(FillBuilder)
									.ExecuteReturnLastId<int>();

			Context.Update("Product", product)
				.Fill(FillBuilder)
				.Where(x => x.ProductId)
				.Execute();

		}

		public void FillBuilder(IInsertUpdateBuilder<Product> builder)
		{
			builder.Column(x => x.Name);
			builder.Column(x => x.CategoryId);
		}
	}
}

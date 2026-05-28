using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Update
{

	public class AutoMapTests : BaseSqlServerIntegrationTest
	{
		
		public void Enum_test()
		{
			using (var context = Context.UseTransaction(true))
			{
				var product = new ProductWithCategoryEnum();
				product.Name = "Test";
				product.CategoryId = Categories.Movies;
				product.ProductId = context.Insert("Product", product)
										.AutoMap(x => x.ProductId)
										.ExecuteReturnLastId<int>();

				product = context.Sql("select * from Product where ProductId=@0", product.ProductId)
							.QuerySingle<ProductWithCategoryEnum>();

				product.CategoryId = Categories.Books;
				context.Update("Product", product)
					.AutoMap(x => x.ProductId)
					.Where(x => x.ProductId)
					.Execute();

				product = context.Sql("select * from Product where ProductId=@0", product.ProductId)
							.QuerySingle<ProductWithCategoryEnum>();
			}
		}
	}
}

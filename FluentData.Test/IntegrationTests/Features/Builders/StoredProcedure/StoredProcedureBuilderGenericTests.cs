using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.StoredProcedure
{

	public class StoredProcedureGenericTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_No_Automap()
		{
			var product = new Product();
			product.Name = "TestProduct";
			product.Category = new Category();
			product.CategoryId = 1;

			using (var context = Context.UseTransaction(true))
			{
				var storedProcedure = context.StoredProcedure("ProductInsert", product)
										.ParameterOut("ProductId", DataTypes.Int32)
										.Parameter("Name", product.Name)
										.Parameter("CategoryId", product.Category.CategoryId);

				storedProcedure.Execute();
				product.ProductId = storedProcedure.ParameterValue<int>("ProductId");
			}
		}

		
		public void TestAutomap()
		{
			var product = new Product();
			product.Name = "TestProduct";
			product.CategoryId = 1;

			using (var context = Context.UseTransaction(true))
			{
				var storedProcedure = context.StoredProcedure<Product>("ProductInsert", product)
										.ParameterOut("ProductId", DataTypes.Int32)
										.AutoMap(x => x.ProductId, x => x.Category);

				storedProcedure.Execute();
				product.ProductId = storedProcedure.ParameterValue<int>("ProductId");
			}
		}
	}
}

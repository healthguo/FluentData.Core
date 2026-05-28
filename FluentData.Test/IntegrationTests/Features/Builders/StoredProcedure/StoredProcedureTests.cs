using FluentData.Core;

namespace FluentData.Test.IntegrationTests.Features.Builders.StoredProcedure
{

	public class StoredProcedureBuilderTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			using (var context = Context.UseTransaction(true))
			{
				var storedProcedure = context.StoredProcedure("ProductInsert")
										.ParameterOut("ProductId", DataTypes.Int32)
										.Parameter("Name", "TestProduct")
										.Parameter("CategoryId", 1);

				storedProcedure.Execute();
				var productId = storedProcedure.ParameterValue<int>("ProductId");
			}
		}
	}
}

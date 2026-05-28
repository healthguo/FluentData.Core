using FluentData.Core;

namespace FluentData.Test.IntegrationTests.Features.Builders.StoredProcedure
{

	public class ParameterTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_Output()
		{
			var builder = Context.StoredProcedure("TestOutputParameter").ParameterOut("ProductName", DataTypes.String, 50);

			builder.Execute();

			builder.ParameterValue<string>("ProductName");
		}
	}
}

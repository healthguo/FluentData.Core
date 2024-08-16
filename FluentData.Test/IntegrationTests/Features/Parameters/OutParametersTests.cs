using FluentData.Core;

namespace FluentData.Test.IntegrationTests.Features.Parameters
{

	public class OutParametersTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			var command = Context.Sql("select top 1 @CategoryName = Name from Category")
							.ParameterOut("CategoryName", DataTypes.String, 50);
			command.Execute();
			command.ParameterValue<string>("CategoryName");
		}

		
		public void Test_null()
		{
			var command = Context.Sql("select @CategoryName = null")
							 .ParameterOut("CategoryName", DataTypes.String, 50);
			command.Execute();
			command.ParameterValue<string>("CategoryName");
		}
	}
}

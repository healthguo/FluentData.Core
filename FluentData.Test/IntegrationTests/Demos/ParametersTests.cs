using FluentData.Core;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class ParametersTests : BaseSqlServerIntegrationTest
	{
		public void Indexed_parameters()
		{
			Context.Sql("select * from Product where ProductId = @0 or ProductId = @1", 1, 2)
				.QueryMany<dynamic>();
		}

		public void Indexed_parameters_alternative()
		{
			Context.Sql("select * from Product where ProductId = @0 or ProductId = @1").Parameters(1, 2)
				.QueryMany<dynamic>();
		}

		public void IndexedParametersAlternativeInsertQuery()
		{
			Context.Sql(@"insert into Product(Name,CategoryId) values(@0,@1)")
				.Parameters("sampleData", 2)
				.ExecuteReturnLastId<int>();
		}

		public void Named_parameters()
		{
			Context.Sql("select * from Product where ProductId = @ProductId1 or ProductId = @ProductId2")
				.Parameter("ProductId1", 1)
				.Parameter("ProductId2", 2)
				.QueryMany<dynamic>();
		}

		public void List_of_parameters_in_Query()
		{
			List<int> ids = new List<int>() { 1, 2, 3, 4 };
			Context.Sql("select * from Product where ProductId in(@0)", ids)
				.QueryMany<dynamic>();

		}

		public void Out_parameter()
		{
			var command = Context.Sql("select @ProductName = Name from Product where ProductId=1")
							.ParameterOut("ProductName", DataTypes.String, 100);
			command.Execute();
			command.ParameterValue<string>("ProductName");
		}
	}
}

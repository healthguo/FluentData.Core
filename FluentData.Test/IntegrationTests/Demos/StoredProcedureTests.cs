using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class StoredProcedureTests : BaseSqlServerIntegrationTest
	{
		public void Stored_procedure_sql()
		{
			Context.Sql("execute ProductUpdate @ProductId = @0, @Name = @1", 1, "The Warren Buffet Way")
				.Execute();
		}

		public void Stored_procedure_sql_stored_procedure_command_type()
		{
			Context.Sql("ProductUpdate")
				.CommandType(DbCommandTypes.StoredProcedure)
				.Parameter("ProductId", 1)
				.Parameter("Name", "The Warren Buffet Way")
				.Execute();
		}

		public void Stored_procedure_builder()
		{
			Context.StoredProcedure("ProductUpdate")
				.Parameter("Name", "The Warren Buffet Way")
				.Parameter("ProductId", 1)
				.Execute();
		}

		public void StoredProcedure_builder_automapping()
		{
			var product = Context.Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();

			product.Name = "The Warren Buffet Way";

			Context.StoredProcedure("ProductUpdate", product)
				.AutoMap(x => x.CategoryId, x => x.Category)
				.Execute();
		}

		public void StoredProcedure_builder_using_expression()
		{
			var product = Context.Sql("select * from Product where ProductId = 1")
							.QuerySingle<Product>();
			product.Name = "The Warren Buffet Way";

			Context.StoredProcedure("ProductUpdate", product)
				.Parameter(x => x.ProductId)
				.Parameter(x => x.Name)
				.Execute();
		}
	}
}

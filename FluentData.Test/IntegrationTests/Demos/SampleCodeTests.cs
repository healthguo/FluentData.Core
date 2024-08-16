using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
	public class SampleCode : BaseSqlServerIntegrationTest
	{
		public void Get_a_single_product()
		{
			Context.Sql(@"select *	from Product where ProductId = 1")
				.QuerySingle<Product>();
		}

		public void Get_many_products()
		{
			Context.Sql(@"select * from Product")
				.QueryMany<Product>();
		}

		public void Insert_a_new_product()
		{
			Context.Insert("Product")
				.Column("Name", "The Warren Buffet Way")
				.Column("CategoryId", 1)
				.ExecuteReturnLastId<int>();
		}

		public void Insert_a_new_product_sql()
		{
			Context.Sql(@"insert into Product(Name, CategoryId) values('The Warren Buffet Way', 1);")
				.ExecuteReturnLastId<int>();
		}

		public void Update_existing_product()
		{
			Context.Update("Product")
				.Column("Name", "The Warren Buffet Way")
				.Column("CategoryId", 1)
				.Where("ProductId", 1)
				.Execute();
		}

		public void Update_existing_product_sql()
		{
			Context.Sql(@"update Product set Name = 'The Warren Buffet Way', CategoryId = 1 where ProductId = 1")
				.Execute();
		}

		public void Delete_a_product_sql()
		{
			var productId = Context.Insert("Product")
								.Column("Name", "The Warren Buffet Way")
								.Column("CategoryId", 1)
								.ExecuteReturnLastId<int>();

			Context.Sql("delete from Product where ProductId = @0", productId)
				.Execute();
		}

		public void Delete_a_product()
		{
			var productId = Context.Insert("Product")
								.Column("Name", "The Warren Buffet Way")
								.Column("CategoryId", 1)
								.ExecuteReturnLastId<int>();

			Context.Delete("Product")
				.Where("ProductId", productId)
				.Execute();
		}

		public void Parameters_indexed()
		{
			Context.Sql(@"select *	from Product where ProductId = @0", 1)
				.QuerySingle<Product>();
		}

		public void Parameters_named()
		{
			Context.Sql(@"select * from Product where ProductId = @ProductId")
				.Parameter("ProductId", 1)
				.QuerySingle<Product>();
		}
	}
}

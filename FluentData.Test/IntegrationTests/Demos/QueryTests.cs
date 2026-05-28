using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class QueryTests : BaseSqlServerIntegrationTest
	{
		public void Query_many_dynamic()
		{
			Context.Sql("select * from Product")
				.QueryMany<dynamic>();
		}

		public void Query_many_strongly_typed()
		{
			Context.Sql("select * from Product")
				.QueryMany<Product>();
		}

		public void Query_many_strongly_typed_custom_collection()
		{
			Context.Sql("select * from Product")
				.QueryMany<Product, ProductionCollection>();
		}

		public void In_Query()
		{
			var ids = new List<int>() { 1, 2, 3, 4 };

			Context.Sql("select * from Product where ProductId in(@0)", ids)
				.QueryMany<dynamic>();
		}

		public void Complex_Query()
		{
			var products = new List<Product>();
			Context.Sql("select * from Product")
				.QueryComplexMany(products, MapComplexProduct);
		}

		private void MapComplexProduct(IList<Product> products, IDataReader reader)
		{
            var product = new Product
            {
                ProductId = reader.GetInt32("ProductId"),
                Name = reader.GetString("Name")
            };
            products.Add(product);
		}
	}
}

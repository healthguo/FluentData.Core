using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class MapperDocumentCode : BaseSqlServerIntegrationTest
	{
		public void Query_auto_mapping_match()
		{
				Context.Sql(@"select * from Product")
					.QueryMany<Product>();

		}

		public void Query_auto_mapping_custom_collection()
		{
			Context.Sql(@"select * from Product")
				.QueryMany<Product, ProductionCollection>();

		}

		public void Query_auto_mapping_alias_manual()
		{
			Context.Sql(@"select p.*,
						c.CategoryId as Category_CategoryId,
						c.Name as Category_Name
						from Product p
						inner join Category c on p.CategoryId = c.CategoryId")
				.QueryMany<Product>();

		}

		public void Query_custom_mapping_dynamic()
		{
			Context.Sql(@"select * from Product")
				.QueryMany<Product>(Custom_mapper_using_dynamic);

		}

		public void Custom_mapper_using_dynamic(Product product, dynamic row)
		{
			product.ProductId = row.ProductId;
			product.Name = row.Name;
		}

		public void Query_custom_mapping_datareader()
		{
			Context.Sql(@"select * from Product")
				.QueryMany<Product>(Custom_mapper_using_datareader);

		}

		public void Custom_mapper_using_datareader(Product product, IDataReader row)
		{
			product.ProductId = row.GetInt32("ProductId");
			product.Name = row.GetString("Name");
		}
	}
}

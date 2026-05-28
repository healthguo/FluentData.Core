namespace FluentData.Test.IntegrationTests.Features.AutoMapping
{

	public class PropertyWithUnderscoreTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_fieldname_propertyname_with_underscore()
		{
			Context.Sql(@"select top 1 p.ProductId as product_Id,
							p.Name,
							c.CategoryId as Category_Id,
							c.Name as Category_Name
						from Product p
						inner join Category c on p.CategoryId = c.CategoryId")
				.QuerySingle<ProductWithUnderscore>();
		}

		
		public void Test_nested_fieldname_propertyname_with_underscore()
		{
			Context.Sql(@"select top 1 p.ProductId as productId,
								c.CategoryId as T_Category_Category_Id,
								c.Name as T_Category_Category_Name
							from Product p
							inner join Category c on p.CategoryId = c.CategoryId")
				.QuerySingle<ProductsWithUnderscore2>();
		}

		public class ProductWithUnderscore
		{
			public int Product_Id { get; set; }

			public string Name { get; set; }

			public int Category_Id { get; set; }

			public string Category_Name { get; set; }
		}

		public class ProductsWithUnderscore2
		{
			public int ProductId { get; set; }

			public CategoryWithUnderscore2 T_Category { get; set; }
		}

		public class CategoryWithUnderscore2
		{
			public int Category_Id { get; set; }

			public string Category_Name { get; set; }	
		}
	}
}

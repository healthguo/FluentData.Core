using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders.Select
{

	public class GeneralTests : BaseSqlServerIntegrationTest
	{
		
		public void Test1()
		{
			Context.Select<Product>("p.ProductId, p.Name, c.CategoryId as Category_CategoryId, c.Name as Category_Name")
				.From(@"Product p inner join Category c on p.ProductId = c.CategoryId")
				.OrderBy("c.Name")
                .Paging(1, 30)
				.QueryMany();
		}

		
		public void Test2()
		{
			Context.Select<Category>("CategoryId, Name")
				.From("Category")
				.QueryMany();
		}

		
		public void Test3()
		{
			Context.Select<Category>("CategoryId, Name")
				.From("Category")
				.Where("CategoryId = @CategoryId")
				.Parameter("CategoryId", 1)
				.QuerySingle();
		}

		
		public void Test_Paging()
		{
			Context.Select<Category>("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
				.Paging(1, 1)
				.QuerySingle();

			Context.Select<Category>("CategoryId, Name")
				.From("Category")
				.OrderBy("Name asc")
                .Paging(2, 1)
				.QuerySingle();
		}

		
		public void Test4_Manual_mapping()
		{
			Context.Select<Category>("c.CategoryId, c.Name")
				.From(@"Product p inner join Category c on p.ProductId = c.CategoryId")
                .OrderBy("c.Name")
				.QueryMany();
		}

		public void Test_GroupBy()
		{
			Context.Select<Product>("c.Name")
                .Select("count(*) as Products")
				.QueryMany();
		}
		
		
		public void Test_WhereOr()
		{
			Context.Select<Category>("CategoryId, Name")
			    .From("Category")
			    .Where("CategoryId = 1")
			    .OrWhere("CategoryId = 2")
				.QueryMany();
		}

		
		public void Test_WhereAnd()
		{
			Context.Select<Category>("CategoryId, Name")
				.From("Category")
				.Where("CategoryId = 1")
				.AndWhere("CategoryId = 1")
				.QueryMany();
		}

		
		public void Test_dynamic()
		{
			Context.Select<dynamic>("CategoryId, Name")
				.From("Category")
				.QueryMany();
		}
	}
}

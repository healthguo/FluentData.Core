using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Queries
{

	public class QueryComplexSingleTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_map_using_data_reader()
		{
			Context.Sql("select top 1 * from Category")
				.QueryComplexSingle(MapCategoryReader);
		}

		private Category MapCategoryReader(IDataReader reader)
		{
			var category = new Category();
			category.CategoryId = (Categories) reader.GetInt32("CategoryId");
			category.Name = reader.GetString("Name");
			return category;
		}

		
		public void Test_map_using_dynamic()
		{
			Context.Sql("select top 1 * from Category")
				.QueryComplexSingle(MapCategoryDynamic);
		}

		private Category MapCategoryDynamic(dynamic reader)
		{
			var category = new Category();
			category.CategoryId = (Categories)reader.CategoryId;
			category.Name = reader.Name;
			return category;
		}
	}
}

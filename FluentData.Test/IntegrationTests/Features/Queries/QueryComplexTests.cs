using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Queries
{

	public class QueryComplexTests : BaseSqlServerIntegrationTest
	{
		
		public void Test()
		{
			var categories = new List<Category>();
			Context.Sql("select * from Category")
				.QueryComplexMany(categories, MapCategory);
		}

		private void MapCategory(IList<Category> categories, IDataReader reader)
		{
			var category = new Category();
			category.CategoryId = (Categories) reader.GetInt32("CategoryId");
			category.Name = reader.GetString("Name");
			categories.Add(category);
		}
	}
}

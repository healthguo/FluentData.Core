using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;
using System.Dynamic;

namespace FluentData.Test.IntegrationTests.Features.Queries
{

	public class QuerySingleNoAutoMapTests : BaseSqlServerIntegrationTest
	{
		
		public void Test_map_using_data_reader()
		{
			Context.Sql("select * from Category")
				.QueryMany<Category>(MapCategoryReader);
		}

        public void Test()
        {
            Context.StoredProcedure("select * from Category")
				.QueryMany<Category>(MapCategoryTest);

            dynamic proc = new ExpandoObject();
            proc.ProductId = 1;
            proc.Name = "Test";

           
        }

	    private void MapCategoryTest(Category arg1, dynamic arg2)
	    {
	    }


	    private void MapCategoryReader(Category category, IDataReader reader)
		{
			category.CategoryId = (Categories)reader.GetInt32("CategoryId");
			category.Name = reader.GetString("Name");
		}

		
		public void Test_map_using_dynamic()
		{
			Context.Sql("select * from Category")
				.QueryMany<Category>(MapCategoryDynamic);
		}

		private void MapCategoryDynamic(Category category, dynamic reader)
		{
			category.CategoryId = (Categories)reader.CategoryId;
			category.Name = reader.Name;
		}
	}
}

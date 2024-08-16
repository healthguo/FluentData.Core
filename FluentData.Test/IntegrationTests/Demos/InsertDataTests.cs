using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class InsertDataTests : BaseSqlServerIntegrationTest
    {
        public void Insert_data_sql()
        {
            Context.Sql("insert into Product(Name, CategoryId) values(@0, @1);", "The Warren Buffet Way", 1)
                .ExecuteReturnLastId<int>();

        }

        public void Insert_data_builder_no_automapping()
        {
            Context.Insert("Product")
                .Column("Name", "The Warren Buffet Way")
                .Column("CategoryId", 1)
                .ExecuteReturnLastId<int>();
        }

        public void Insert_data_builder_automapping()
        {
            var product = new Product
            {
                Name = "The Warren Buffet Way",
                CategoryId = 1
            };

            product.ProductId = Context.Insert("Product", product)
                                    .AutoMap(x => x.ProductId, x => x.Category)
                                    .ExecuteReturnLastId<int>();

        }
    }
}

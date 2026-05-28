using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Demos
{
    public class UpdateDataTests : BaseSqlServerIntegrationTest
    {
        public void Update_data_sql()
        {
            Context.Sql("update Product set Name = @0 where ProductId = @1", "The Warren Buffet Way", 1)
                .Execute();
        }

        public void Update_data_builder()
        {
            Context.Update("Product")
                .Column("Name", "The Warren Buffet Way")
                .Where("ProductId", 1)
                .Execute();
        }

        public void Update_data_builder_automapping()
        {
            Product product = Context.Sql("select * from Product where ProductId = 1")
                                .QuerySingle<Product>();
            product.Name = "The Warren Buffet Way";

            Context.Update("Product", product)
                .Where(x => x.ProductId)
                .AutoMap(x => x.ProductId, x => x.Category)
                .Execute();
        }
    }
}

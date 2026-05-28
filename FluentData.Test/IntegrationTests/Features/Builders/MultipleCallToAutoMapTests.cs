using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders
{

    public class MultipleCallToAutoMapTests : BaseSqlServerIntegrationTest
    {

        public void InsertBuilder_AutoMapIsCalledMultipleTimes_Throw()
        {
            var product = new Product();
            product.Name = "Test";
            Context.Insert("Product", product).AutoMap().AutoMap();
        }


        public void UpdateBuilder_AutoMapIsCalledMultipleTimes_Throw()
        {
            var product = new Product();
            product.Name = "Test";
            Context.Update("Product", product).AutoMap().AutoMap();
        }
    }
}
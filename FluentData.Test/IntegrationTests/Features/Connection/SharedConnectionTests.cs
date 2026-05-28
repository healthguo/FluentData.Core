namespace FluentData.Test.IntegrationTests.Features.Connection
{

    public class SharedConnectionTests : BaseSqlServerIntegrationTest
    {

        public void Test_shared_connection()
        {
            using (var context = Context.UseSharedConnection(true))
            {
                context.Sql("select top 1 * from category").QuerySingle<dynamic>();

                context.Sql("select top 1 * from category").QuerySingle<dynamic>();
            }
        }
    }
}

namespace FluentData.Test.IntegrationTests.Features.Transaction
{

    public class RollbackUpdatedData : BaseSqlServerIntegrationTest
    {
        
        public void Update_data_rollback()
        {
            using (var db = Context.UseTransaction(true))
            {
                var category = db.Sql("select * from Category where CategoryId = 1").QuerySingle<dynamic>();

                var affectedRows = db.Sql("update Category set Name = 'BooksChanged' where CategoryId=1").Execute();

                var updatedCategory = db.Sql("select * from Category where CategoryId = 1").QuerySingle<dynamic>();
            }

            Context.Sql("select * from Category where CategoryId = 1").QuerySingle<dynamic>();
        }
    }
}

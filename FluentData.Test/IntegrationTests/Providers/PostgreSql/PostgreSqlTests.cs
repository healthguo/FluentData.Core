using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;
using FluentData.Test.IntegrationTests.Providers.MySql;

namespace FluentData.Test.IntegrationTests.Providers.PostgreSql
{


    public class PostgreSqlTests : IDbProviderTests
    {
        public PostgreSqlTests()
        {
            Context.Sql(@"drop table if exists Category;
							drop table if exists Product;
							drop function if exists ProductUpdate(integer, varchar);

							CREATE TABLE Category(
								CategoryId INTEGER PRIMARY KEY,
								Name VARCHAR(50) not null);

							CREATE TABLE Product(
								ProductId serial primary key,
								Name VARCHAR(50) not null,
								CategoryId INTEGER not null);

							insert into Category(CategoryId, Name)
							select 1, 'Books'
							union select 2, 'Movies';

							insert into Product(Name, CategoryId)
							select 'The Warren Buffet Way', 1
							union select 'Bill Gates Bio', 1
							union select 'James Bond - Goldeneye', 2
							union select 'The Bourne Identity', 2;

							CREATE FUNCTION ProductUpdate(ParamProductId integer, ParamName varchar(100)) RETURNS void AS $$
							BEGIN
								update Product set Name = ParamName where ProductId = ParamProductId;
							END;
							$$ LANGUAGE plpgsql;
							").Execute();
        }

        protected IDbContext Context
        {
            get
            {
                return new DbContext().ConnectionString(TestHelper.GetConnectionStringValue("PostgreSql"),
                                                        new PostgreSqlProvider());
            }
        }


        public void Query_many_dynamic()
        {
            Context.Sql("select * from Product")
                .QueryMany<dynamic>();
        }


        public void Query_single_dynamic()
        {
            Context.Sql("select * from Product where ProductId = 1")
                .QuerySingle<dynamic>();
        }


        public void Query_many_strongly_typed()
        {
            Context.Sql("select * from Product")
                .QueryMany<Product>();
        }


        public void Query_single_strongly_typed()
        {
            Context.Sql("select * from Product where ProductId = 1")
                .QuerySingle<Product>();
        }


        public void Query_auto_mapping_alias()
        {
            Context.Sql(@"select p.*,
							c.CategoryId as Category_CategoryId,
							c.Name as Category_Name
							from Product p
							inner join Category c on p.CategoryId = c.CategoryId
							where ProductId = 1")
                .QuerySingle<Product>();
        }


        public void Query_custom_mapping_dynamic()
        {
            Context.Sql(@"select * from Product")
                .QueryMany<Product>(Custom_mapper_using_dynamic);
        }

        public void Custom_mapper_using_dynamic(Product product, dynamic row)
        {
            product.ProductId = row.ProductId;
            product.Name = row.Name;
        }


        public void Query_custom_mapping_datareader()
        {
            Context.Sql(@"select * from Product")
                .QueryMany<Product>(Custom_mapper_using_datareader);
        }

        public void Custom_mapper_using_datareader(Product product, IDataReader row)
        {
            product.ProductId = row.GetInt32("ProductId");
            product.Name = row.GetString("Name");
        }


        public void QueryValue()
        {
            Context.Sql("select CategoryId from Product where ProductId = 1")
                .QuerySingle<int>();
        }


        public void QueryValues()
        {
            Context.Sql("select CategoryId from Category order by CategoryId")
                .QueryMany<int>();
        }


        public void Unnamed_parameters_one()
        {
            Context.Sql("select * from Product where ProductId = :0", 1)
                .QuerySingle<dynamic>();
        }


        public void Unnamed_parameters_many()
        {
            Context.Sql("select * from Product where ProductId = :0 or ProductId = :1", 1, 2)
                .QueryMany<dynamic>();
        }


        public void Named_parameters()
        {
            Context.Sql("select * from Product where ProductId = :ProductId1 or ProductId = :ProductId2")
                .Parameter("ProductId1", 1)
                .Parameter("ProductId2", 2)
                .QueryMany<dynamic>();
        }


        public void In_Query()
        {
            var ids = new List<int>() { 1, 2, 3, 4 };

            Context.Sql("select * from Product where ProductId in(:0)", ids)
                .QueryMany<dynamic>();
        }


        public void SelectBuilder_Paging()
        {
            var category = Context
                .Select<Category>("CategoryId, Name")
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


        public void MultipleResultset()
        {
            using (var command = Context.MultiResultSql)
            {
                command.Sql(@"select * from Category;select * from Product;")
                    .QueryMany<dynamic>();
                command.QueryMany<dynamic>();
            }
        }


        public void Insert_data_sql()
        {
            Context.Sql("insert into Product(Name, CategoryId) values(:0, :1);", "The Warren Buffet Way", 1)
                .ExecuteReturnLastId<int>();
        }


        public void Insert_data_builder_no_automapping()
        {
            Context.Insert("Product")
                .Column("CategoryId", 1)
                .Column("Name", "The Warren Buffet Way")
                .ExecuteReturnLastId<int>();
        }


        public void Insert_data_builder_automapping()
        {
            var product = new Product();
            product.CategoryId = 1;
            product.Name = "The Warren Buffet Way";

            Context.Insert("Product", product)
                .AutoMap(x => x.ProductId)
                .ExecuteReturnLastId<int>();
        }


        public void Update_data_sql()
        {
            Context.Sql("update Product set Name = :0 where ProductId = :1", "The Warren Buffet Way", 1)
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
            var product = Context.Sql("select * from Product where ProductId = 1")
                              .QuerySingle<Product>();

            product.Name = "The Warren Buffet Way";

            Context.Update("Product", product)
                .Where(x => x.ProductId)
                .AutoMap()
                .Execute();
        }


        public void Delete_data_sql()
        {
            var productId = Context.Sql("insert into Product(Name, CategoryId) values(:0, :1);", "The Warren Buffet Way", 1)
                                .ExecuteReturnLastId<int>();

            Context.Sql("delete from Product where ProductId = :0", productId)
                .Execute();
        }


        public void Delete_data_builder()
        {
            var productId = Context.Sql(@"insert into Product(Name, CategoryId) values(:0, :1)", "The Warren Buffet Way", 1)
                .ExecuteReturnLastId<int>();

            Context.Delete("Product")
                .Where("ProductId", productId)
                .Execute();
        }


        public void Transactions()
        {
            using (var context = Context.UseTransaction(true))
            {
                context.Sql("update Product set Name = :0 where ProductId = :1", "The Warren Buffet Way", 1)
                    .Execute();

                context.Sql("update Product set Name = :0 where ProductId = :1", "Bill Gates Bio", 2)
                    .Execute();

                context.Commit();
            }
        }


        public void Stored_procedure_sql()
        {
            Context.Sql("ProductUpdate")
                .CommandType(DbCommandTypes.StoredProcedure)
                .Parameter("ParamProductId", 1)
                .Parameter("ParamName", "The Warren Buffet Way")
                .Execute();
        }


        public void Stored_procedure_builder()
        {
            Context.StoredProcedure("ProductUpdate")
                .Parameter("ParamProductId", 1)
                .Parameter("ParamName", "The Warren Buffet Way")
                .Execute();
        }


        public void StoredProcedure_builder_automapping()
        {
            var product = Context.Sql("select * from Product where ProductId = 1")
                             .QuerySingle<Product>();
            product.Name = "The Warren Buffet Way";

            var mysqlProduct = new MySqlProduct(product);

            Context.StoredProcedure("ProductUpdate", mysqlProduct)
                .AutoMap(x => x.ParamCategoryId)
                .Execute();
        }


        public void StoredProcedure_builder_using_expression()
        {
            var product = Context.Sql("select * from Product where ProductId = 1")
                            .QuerySingle<Product>();
            product.Name = "The Warren Buffet Way";

            var mysqlProduct = new MySqlProduct(product);
            Context.StoredProcedure("ProductUpdate", mysqlProduct)
                .Parameter(x => x.ParamProductId)
                .Parameter(x => x.ParamName)
                .Execute();
        }
    }
}

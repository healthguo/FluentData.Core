# FluentData.Core

FluentData.Core - a micro-ORM with a fluent API that makes it simple to query a database.

---

A simple-to-use micro-ORM with a great fluent API that makes it simple to select, insert, update, and delete data in a database.

# Getting started

### Requirements

- .NET 6.0 or newer

### Supported databases

- MS SQL Server through the [System.Data.SqlClient](https://www.nuget.org/packages/System.Data.SqlClient).
- Oracle through the [Oracle.ManagedDataAccess.Core](https://www.nuget.org/packages/Oracle.ManagedDataAccess.Core).
- MySQL through the [MySql.Data](https://www.nuget.org/packages/MySql.Data).
- SQLite through the [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core).
- MS Access through the [System.Data.OleDb](https://www.nuget.org/packages/System.Data.OleDb).
- Dameng through the [dmdbms.DmProvider](https://www.nuget.org/packages/dmdbms.dmprovider).
- PostgreSql through the [Npgsql](https://www.nuget.org/packages/Npgsql).
- IBM DB2 through the [IBM.Data.DB2.Core](https://www.nuget.org/packages/IBM.Data.DB2.Core).

### Installation

If you are using [NuGet](https://www.nuget.org/packages/FluentData.Core/):

```cs
PM > Install-Package FluentData.Core
```

If you are not using NuGet:

1.  Download the zip with the binary files.
2.  Extract it, and copy the files to your solution or project folder.
3.  Add a project reference to FluentData.Core.dll.

# Core concepts

#### DbContext

This class is the starting point for working with FluentData.Core. It has properties for defining configurations such as the connection string to the database, and operations for querying the database.

#### DbCommand

This is the class that is responsible for performing the actual query against the database.

#### Events

The DbContext class has support for the following events:

- OnConnectionOpening
- OnConnectionOpened
- OnConnectionClosed
- OnExecuting
- OnExecuted
- OnError

By using any of these then you can for instance write to the log if an error has occurred or when a query has been executed.

#### Builders

A builder provides a nice fluent API for generating SQL for insert, update and delete queries.

#### Mapping

FluentData.Core can automap the result from a SQL query to either a dynamic type or to your own .NET entity type by using the following convention:

##### Automap to an entity type:

1.  If the field name does not contain an underscore ("\_") then it will try to try to automap to a property with the same name. For instance a field named "Name" would be automapped to a property also named "Name".
2.  If a field name does contain an underscore ("\_") then it will try to map to a nested property. For instance a field named "Category_Name" would be automapped to the property "Category.Name".

If there is a mismatch between the fields in the database and in the entity type then the alias keyword in SQL can be used or you can create your own mapping method. Check the mapping section below for code samples.

##### Automap to a dynamic type:

For dynamic types every field will be automapped to a property with the same name. For instance the field name Name would be automapped to the Name property.

#### When should you dispose?

- DbContext must be disposed if you have enabled UseTransaction or UseSharedConnection.
- DbCommand must be disposed if you have enabled UseMultiResult (or MultiResultSql).
- StoredProcedureBuilder must be disposed if you have enabled UseMultiResult.

In all the other cases dispose will be handled automatically by FluentData.Core. This means that a database connection is opened just before a query is executed and closed just after the execution has been completed.

# Code samples

### Create and initialize a DbContext

The connection string on the DbContext class can be initialized either by giving the connection string name in the \*.config file or by sending in the entire connection string.

#### Important configurations

##### `IgnoreIfAutoMapFails`

Calling this prevents automapper from throwing an exception if a column cannot be mapped to a corresponding property due to a name mismatch.

#### Create and initialize a DbContext

The DbContext can be initialized by either calling ConnectionStringName which will read the connection string from the \*.config file:

```cs
public IDbContext Context()
{
    return new DbContext().ConnectionStringName("MyDatabase", new SqlServerProvider());
}
```

or by calling the ConnectionString method to set the connection string explicitly:

```cs
public IDbContext Context()
{
    return new DbContext().ConnectionString("Server=MyServerAddress;Database=MyDatabase;Trusted_Connection=True;", new SqlServerProvider());
}
```

#### Providers

If you want to work against another database than SqlServer then simply replace the SqlServerProvider in the sample code above with any of the following:

- SqlServerProvider
- OracleProvider
- MySqlProvider
- SqliteProvider
- AccessProvider
- DmProvider
- PostgreSqlProvider
- DB2Provider
- SqlAzureProvider
- SqlServerCompactProvider

#### Query for a list of items

##### Return a list of dynamic objects:

```cs
List<dynamic> products = Context.Sql(@"SELECT * FROM Product").QueryMany<dynamic>();
```

##### Return a list of strongly typed objects:

```cs
List<Product> products = Context.Sql(@"SELECT * FROM Product").QueryMany<Product>();
```

##### Return a list of strongly typed objects in a custom collection:

```cs
ProductionCollection products = Context.Sql(@"SELECT * FROM Product").QueryMany<Product, ProductionCollection>();
```

##### Return a DataTable:

See [Query for a single item](#QuerySingle).

#### Query for a single item

Return as a dynamic object:

```cs
dynamic product = Context.Sql(@"SELECT * FROM Product WHERE ProductId = 1").QuerySingle<dynamic>();
```

##### Return as a strongly typed object:

```cs
Product product = Context.Sql(@"SELECT * FROM Product WHERE ProductId = 1").QuerySingle<Product>();
```

##### Return as a DataTable:

```cs
DataTable products = Context.Sql(@"SELECT * FROM Product").QueryDataTable();

DataTable products = Context.Sql(@"SELECT * FROM Product").QuerySingle<DataTable>();
```

Both QueryMany<DataTable> and QuerySingle<DataTable> can be called to return a DataTable, but since QueryMany returns a List<DataTable> then it's more convenient to call QuerySingle which returns just DataTable. Even though the method is called QuerySingle, multiple rows will still be returned as part of the DataTable.

#### Query for a scalar value

```cs
int numberOfProducts = Context.Sql(@"SELECT COUNT(*) FROM Product").QuerySingle<int>();
```

#### Query for a list of scalar values

```cs
List<int> productIds = Context.Sql(@"SELECT ProductId FROM Product").QueryMany<int>();
```

#### Parameters

##### Indexed parameters:

```cs
dynamic products = Context.Sql(@"SELECT * FROM Product WHERE ProductId = @0 OR ProductId = @1", 1, 2)
    .QueryMany<dynamic>();
```

or:

```cs
dynamic products = Context.Sql(@"SELECT * FROM Product WHERE ProductId = @0 OR ProductId = @1")
    .Parameters(1, 2)
    .QueryMany<dynamic>();
```

##### Named parameters:

```cs
dynamic products = Context.Sql(@"SELECT * FROM Product WHERE ProductId = @ProductId1 OR ProductId = @ProductId2")
    .Parameter("ProductId1", 1)
    .Parameter("ProductId2", 2)
    .QueryMany<dynamic>();
```

##### Output parameter:

```cs
var command = Context.Sql(@"SELECT @ProductName = Name FROM Product WHERE ProductId=1")
    .ParameterOut("ProductName", DataTypes.String, 100);
command.Execute();

string productName = command.ParameterValue<string>("ProductName");
```

##### List of parameters - `IN` operator:

```cs
List<int> ids = new List<int>() { 1, 2, 3, 4 };
//be careful here; don't leave any whitespace around IN(...) syntax.
dynamic products = Context.Sql(@"SELECT * FROM Product WHERE ProductId IN(@0)", ids)
    .QueryMany<dynamic>();
```

##### `LIKE` operator:

```cs
string cens = "%abc%";
Context.Sql(@"SELECT * FROM Product WHERE ProductName LIKE @0", cens);
```

#### Mapping

##### Automapping - 1:1 match between the database and the .NET object:

```cs
List<Product> products = Context.Sql(@"SELECT * FROM Product")
    .QueryMany<Product>();
```

##### Automap to a custom collection:

```cs
ProductionCollection products = Context.Sql(@"SELECT * FROM Product")
    .QueryMany<Product, ProductionCollection>();
```

##### Automapping - Mismatch between the database and the .NET object, use the alias keyword in SQL:

##### Weakly typed:

```cs
List<Product> products = Context.Sql(@"SELECT p.*,
    c.CategoryId AS Category_CategoryId,
    c.Name AS Category_Name
    FROM Product p
    INNER JOIN Category c ON p.CategoryId = c.CategoryId")
    .QueryMany<Product>();
```

Here the p.\* which is ProductId and Name would be automapped to the properties Product.Name and Product.ProductId, and Category_CategoryId and Category_Name would be automapped to Product.Category.CategoryId and Product.Category.Name.

##### Custom mapping using dynamic:

```cs
List<Product> products = Context.Sql(@"SELECT * FROM Product")
    .QueryMany<Product>(Custom_mapper_using_dynamic);

public void Custom_mapper_using_dynamic(Product product, dynamic row)
{
    product.ProductId = row.ProductId;
    product.Name = row.Name;
}
```

##### Custom mapping using a datareader:

```cs
List<Product> products = Context.Sql(@"SELECT * FROM Product")
    .QueryMany<Product>(Custom_mapper_using_datareader);

public void Custom_mapper_using_datareader(Product product, IDataReader row)
{
    product.ProductId = row.GetInt32("ProductId");
    product.Name = row.GetString("Name");
}
```

Or if you have a complex entity type where you need to control how it is created then the QueryComplexMany/QueryComplexSingle can be used:

```cs
var products = new List<Product>();
Context.Sql(@"SELECT * FROM Product")
    .QueryComplexMany<Product>(products, MapComplexProduct);

private void MapComplexProduct(IList<Product> products, IDataReader reader)
{
    var product = new Product();
    product.ProductId = reader.GetInt32("ProductId");
    product.Name = reader.GetString("Name");
    products.Add(product);
}
```

#### Multiple result sets

FluentData.Core supports multiple resultsets. This allows you to do multiple queries in a single database call. When this feature is used it's important to wrap the code inside a using statement as shown below in order to make sure that the database connection is closed.

```cs
using (var command = Context.MultiResultSql)
{
    List<Category> categories = command.Sql(@"SELECT * FROM Category; SELECT * FROM Product;")
        .QueryMany<Category>();

    List<Product> products = command.QueryMany<Product>();
}
```

The first time the Query method is called it does a single query against the database. The second time the Query is called, FluentData.Core already knows that it's running in a multiple result set mode, so it reuses the data retrieved from the first query.

#### Select data and Paging

A select builder exists to make selecting data and paging easy:

```cs
List<Product> products = Context.Select<Product>(@"p.*, c.Name AS Category_Name")
    .From(@"Product p INNER JOIN Category c ON c.CategoryId = p.CategoryId")
    .Where(@"p.ProductId > 0 AND p.Name IS NOT NULL")
    .OrderBy("p.Name")
    .Paging(1, 10)
    .QueryMany();
```

By calling Paging(1, 10) then the first 10 products will be returned.

#### Insert data

##### Using SQL:

```cs
int productId = Context.Sql(@"INSERT INTO Product(Name, CategoryId) VALUES(@0, @1);")
    .Parameters("The Warren Buffet Way", 1)
    .ExecuteReturnLastId<int>();
```

##### Using a builder:

```cs
int productId = Context.Insert("Product")
    .Column("Name", "The Warren Buffet Way")
    .Column("CategoryId", 1)
    .ExecuteReturnLastId<int>();
```

##### Using a builder with automapping:

```cs
Product product = new Product();
product.Name = "The Warren Buffet Way";
product.CategoryId = 1;

product.ProductId = Context.Insert<Product>("Product", product)
    .AutoMap(x => x.ProductId)
    .ExecuteReturnLastId<int>();

//table name is the same as class name
product.ProductId = Context.Insert<Product>(product)
    .AutoMap(x => x.ProductId)
    .ExecuteReturnLastId<int>();
```

We send in ProductId to the AutoMap method to get AutoMap to ignore and not map the ProductId since this property is an identity field where the value is generated in the database.

#### Update data

##### Using SQL:

```cs
int rowsAffected = Context.Sql(@"UPDATE Product SET Name = @0 WHERE ProductId = @1")
    .Parameters("The Warren Buffet Way", 1)
    .Execute();
```

##### Using a builder:

```cs
int rowsAffected = Context.Update("Product")
    .Column("Name", "The Warren Buffet Way")
    .Where("ProductId", 1)
    .Execute();
```

##### Using a builder with automapping:

```cs
Product product = Context.Sql(@"SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();
product.Name = "The Warren Buffet Way";

int rowsAffected = Context.Update<Product>("Product", product)
    .AutoMap(x => x.ProductId)
    .Where(x => x.ProductId)
    .Execute();
```

We send in ProductId to the AutoMap method to get AutoMap to ignore and not map the ProductId since this is the identity field that should not get updated.

#### IgnoreIfAutoMapFails

When reading from the database, if some data columns are not mapped to an entity class, FluentData.Core will (by default) throw an exception. If you want ignore the exception, or if the property is not used for map data table, then you can use the `IgnoreIfAutoMapFails(true);` option; this will ignore the exception.

```cs
context.IgnoreIfAutoMapFails(true);
```

##### Insert and update - common Fill method

```cs
var product = new Product();
product.Name = "The Warren Buffet Way";
product.CategoryId = 1;

Context.Insert<Product>("Product", product)
.Fill(FillBuilder)
.Execute();

Context.Update<Product>("Product", product)
.Fill(FillBuilder)
.Execute();

public void FillBuilder(IInsertUpdateBuilder<Product> builder)
{
    builder.Column(x => x.Name);
    builder.Column(x => x.CategoryId);
}
```

#### Delete data

##### Using SQL:

```cs
int rowsAffected = Context.Sql(@"DELETE FROM Product WHERE ProductId = 1")
    .Execute();
```

##### Using a builder:

```cs
int rowsAffected = Context.Delete("Product")
    .Where("ProductId", 1)
    .Execute();
```

#### Stored procedure

##### Using SQL:

```cs
var rowsAffected = Context.Sql("ProductUpdate")
    .CommandType(DbCommandTypes.StoredProcedure)
    .Parameter("ProductId", 1)
    .Parameter("Name", "The Warren Buffet Way")
    .Execute();
```

##### Using a builder:

```cs
var rowsAffected = Context.StoredProcedure("ProductUpdate")
    .Parameter("Name", "The Warren Buffet Way")
    .Parameter("ProductId", 1)
    .Execute();
```

##### Using a builder with automapping:

```cs
var product = Context.Sql(@"SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();

product.Name = "The Warren Buffet Way";

var rowsAffected = Context.StoredProcedure<Product>("ProductUpdate", product)
    .AutoMap(x => x.CategoryId)
    .Execute();
```

##### Using a builder with automapping and expressions:

```cs
var product = Context.Sql(@"SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();
product.Name = "The Warren Buffet Way";

var rowsAffected = Context.StoredProcedure<Product>("ProductUpdate", product)
    .Parameter(x => x.ProductId)
    .Parameter(x => x.Name)
    .Execute();
```

#### Transactions

FluentData.Core supports transactions. When you use transactions its important to wrap the code inside a using statement to make sure that the database connection is closed. By default, if any exception occur or if Commit is not called then Rollback will automatically be called.

```cs
using (var context = Context.UseTransaction(true))
{
    context.Sql(@"UPDATE Product SET Name = @0 WHERE ProductId = @1")
        .Parameters("The Warren Buffet Way", 1)
        .Execute();

    context.Sql(@"UPDATE Product SET Name = @0 WHERE ProductId = @1")
        .Parameters("Bill Gates Bio", 2)
        .Execute();

    context.Commit();
}
```

#### Entity factory

The entity factory is responsible for creating object instances during automapping. If you have some complex business objects that require special actions during creation, you can create your own custom entity factory:

```cs
List<Product> products = Context.EntityFactory(new CustomEntityFactory())
    .Sql(@"SELECT * FROM Product")
    .QueryMany<Product>();

public class CustomEntityFactory : IEntityFactory
{
    public object Create(Type type)
    {
        return Activator.CreateInstance(type);
    }
}
```

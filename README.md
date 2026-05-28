# FluentData.Core

A micro-ORM with a fluent API that simplifies database queries in .NET.

---

## Table of Contents

- [Getting started](#getting-started)
  - [Requirements](#requirements)
  - [Supported Databases](#supported-databases)
  - [Installation](#installation)
    - [Using NuGet](#using-nuget)
    - [Manual Installation](#manual-installation)
- [Core concepts](#core-concepts)
  - [DbContext](#dbcontext)
  - [DbCommand](#dbcommand)
  - [Events](#events)
  - [Builders](#builders)
  - [Mapping](#mapping)
    - [Automap to an entity type](#automap-to-an-entity-type)
    - [Automap to a dynamic type](#automap-to-a-dynamic-type)
  - [When should you dispose?](#when-should-you-dispose)
- [Code samples](#code-samples)
  - [Create and initialize a DbContext](#create-and-initialize-a-dbcontext)
    - [Important configurations](#important-configurations)
    - [Using Dependency Injection](#using-dependency-injection)
    - [Using DbContext Directly](#using-dbcontext-directly)
    - [Providers](#providers)
  - [Query for a list of items](#query-for-a-list-of-items)
    - [Return a list of dynamic objects](#return-a-list-of-dynamic-objects)
    - [Return a list of strongly typed objects](#return-a-list-of-strongly-typed-objects)
    - [Return a list in a custom collection](#return-a-list-in-a-custom-collection)
  - [Query for a single item](#query-for-a-single-item)
    - [Return as a dynamic object](#return-as-a-dynamic-object)
    - [Return as a strongly typed object](#return-as-a-strongly-typed-object)
    - [Return as a DataTable](#return-as-a-datatable)
  - [Query for a scalar value](#query-for-a-scalar-value)
  - [Query for a list of scalar values](#query-for-a-list-of-scalar-values)
  - [Parameters](#parameters)
    - [Indexed parameters](#indexed-parameters)
    - [Named parameters](#named-parameters)
    - [Output parameter](#output-parameter)
    - [List of parameters - IN operator](#list-of-parameters---in-operator)
    - [LIKE operator](#like-operator)
  - [Mapping](#mapping-1)
    - [Automapping - 1:1 match](#automapping---11-match)
    - [Automap to a custom collection](#automap-to-a-custom-collection)
    - [Automapping - Mismatch resolution using SQL alias](#automapping---mismatch-resolution-using-sql-alias)
    - [Custom mapping using dynamic](#custom-mapping-using-dynamic)
    - [Custom mapping using IDataReader](#custom-mapping-using-idatareader)
    - [Complex entity mapping](#complex-entity-mapping)
  - [Multiple result sets](#multiple-result-sets)
  - [Select data and Paging](#select-data-and-paging)
  - [Insert data](#insert-data)
    - [Using SQL](#using-sql)
    - [Using a builder](#using-a-builder)
    - [Using a builder with automapping](#using-a-builder-with-automapping)
  - [Update data](#update-data)
    - [Using SQL](#using-sql-1)
    - [Using a builder](#using-a-builder-1)
    - [Using a builder with automapping](#using-a-builder-with-automapping-1)
    - [Insert and update - common Fill method](#insert-and-update---common-fill-method)
  - [Delete data](#delete-data)
    - [Using SQL](#using-sql-2)
    - [Using a builder](#using-a-builder-2)
  - [Stored procedure](#stored-procedure)
    - [Using SQL](#using-sql-3)
    - [Using a builder](#using-a-builder-3)
    - [Using a builder with automapping](#using-a-builder-with-automapping-2)
    - [Using a builder with expressions](#using-a-builder-with-expressions)
  - [Transactions](#transactions)
  - [Entity factory](#entity-factory)

---

## Getting started

### Requirements

- .NET 8.0 or newer

## Supported Databases

| Database   | NuGet Package                                                                                 | Provider Class       |
| ---------- | --------------------------------------------------------------------------------------------- | -------------------- |
| SQL Server | [Microsoft.Data.SqlClient](https://www.nuget.org/packages/Microsoft.Data.SqlClient)           | `SqlServerProvider`  |
| Oracle     | [Oracle.ManagedDataAccess.Core](https://www.nuget.org/packages/Oracle.ManagedDataAccess.Core) | `OracleProvider`     |
| MySQL      | [MySql.Data](https://www.nuget.org/packages/MySql.Data)                                       | `MySqlProvider`      |
| SQLite     | [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core)             | `SqliteProvider`     |
| MS Access  | [System.Data.OleDb](https://www.nuget.org/packages/System.Data.OleDb)                         | `AccessProvider`     |
| Dameng     | [dmdbms.DmProvider](https://www.nuget.org/packages/dmdbms.dmprovider)                         | `DmProvider`         |
| PostgreSQL | [Npgsql](https://www.nuget.org/packages/Npgsql)                                               | `PostgreSqlProvider` |
| IBM DB2    | [Net.IBM.Data.Db2](https://www.nuget.org/packages/Net.IBM.Data.Db2)                           | `DB2Provider`        |

### Installation

#### Using NuGet

```powershell
PM > Install-Package FluentData.Core
```

#### Manual Installation

1. Download the zip file with binary files.
2. Extract to your solution or project folder.
3. Add a project reference to `FluentData.Core.dll`.

---

## Core concepts

### DbContext

The starting point for working with FluentData.Core. Use it to configure the connection string and execute queries.

### DbCommand

Executes queries against the database.

### Events

The `DbContext` class supports the following lifecycle events:

| Event                 | Description                                 |
| --------------------- | ------------------------------------------- |
| `OnConnectionOpening` | Fired before a connection is opened         |
| `OnConnectionOpened`  | Fired after a connection is opened          |
| `OnConnectionClosed`  | Fired after a connection is closed          |
| `OnExecuting`         | Fired before a command is executed          |
| `OnExecuted`          | Fired after a command is executed           |
| `OnError`             | Fired when an error occurs during execution |

Use these events for logging errors or tracking query execution.

### Builders

Builders provide a fluent API for generating SQL for insert, update, and delete queries.

### Mapping

FluentData.Core automaps query results to dynamic types or .NET entity types using the following convention:

#### Automap to an entity type

1. **Direct match**: If a field name contains no underscore (`_`), it maps to a property with the same name. Example: field `Name` → property `Name`.
2. **Nested property**: If a field name contains an underscore (`_`), it maps to a nested property. Example: field `Category_Name` → property `Category.Name`.

Use the SQL `AS` keyword or create custom mapping methods for mismatches. See the [Mapping section](#mapping-1) for examples.

#### Automap to a dynamic type

Every field maps to a property with the same name. Example: field `Name` → property `Name`.

### When should you dispose?

- **DbContext**: Required when `UseTransaction` or `UseSharedConnection` is enabled.
- **DbCommand**: Required when `UseMultiResult` (or `MultiResultSql`) is enabled.
- **StoredProcedureBuilder**: Required when `UseMultiResult` is enabled.

In all other cases, FluentData.Core handles disposal automatically. Connections open just before execution and close immediately after.

---

## Code samples

### Create and initialize a DbContext

Initialize the connection string using the connection string name from a config file or by providing the full connection string.

#### Important configurations

##### `IgnoreIfAutoMapFails`

Prevents the automapper from throwing an exception when a column cannot be mapped to a property due to a name mismatch.

```csharp
dbContext.IgnoreIfAutoMapFails(true);
```

#### Using Dependency Injection

```csharp
// Option 1: Direct connection string
builder.Services.AddFluentData("Server=MyServer;Database=MyDb;Trusted_Connection=True;", new SqlServerProvider());

// Option 2: Configuration callback
builder.Services.AddFluentData(ctx =>
{
    ctx.ConnectionString("Server=MyServer;Database=MyDb;Trusted_Connection=True;", new SqlServerProvider());
});

// Option 3: Chained method
builder.Services.AddFluentData()
    .ConnectionString("Server=MyServer;Database=MyDb;Trusted_Connection=True;", new SqlServerProvider());

// Option 4: Connection string name from appsettings.json
builder.Services.AddFluentData()
    .ConnectionStringName(builder.Configuration, "MyDatabase", new SqlServerProvider());
```

Usage:

```csharp
public class MyService(IDbContext dbContext)
{
}
```

#### Using DbContext Directly

```csharp
// Option 1: Direct connection string
public IDbContext Context()
{
    return new DbContext()
        .ConnectionString("Server=MyServer;Database=MyDb;Trusted_Connection=True;", new SqlServerProvider());
}

// Option 2: Connection string name from config file
public IDbContext Context()
{
    return new DbContext()
        .ConnectionStringNameFromConfigFile("MyDatabase", new SqlServerProvider());
}
```

#### Providers

Replace `SqlServerProvider` with any of the following:

- `SqlServerProvider`
- `OracleProvider`
- `MySqlProvider`
- `SqliteProvider`
- `AccessProvider`
- `DmProvider`
- `PostgreSqlProvider`
- `DB2Provider`
- `SqlAzureProvider`
- `SqlServerCompactProvider`

### Query for a list of items

#### Return a list of dynamic objects

```csharp
List<dynamic> products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<dynamic>();
```

#### Return a list of strongly typed objects

```csharp
List<Product> products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<Product>();
```

#### Return a list in a custom collection

```csharp
ProductionCollection products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<Product, ProductionCollection>();
```

### Query for a single item

#### Return as a dynamic object

```csharp
dynamic product = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<dynamic>();
```

#### Return as a strongly typed object

```csharp
Product product = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();
```

#### Return as a DataTable

```csharp
DataTable products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryDataTable();

// Or
DataTable products = dbContext
    .Sql("SELECT * FROM Product")
    .QuerySingle<DataTable>();
```

Both `QueryMany<DataTable>` and `QuerySingle<DataTable>` return a DataTable. `QuerySingle` is more convenient since it returns `DataTable` directly instead of `List<DataTable>`. Despite the name, multiple rows are still returned.

### Query for a scalar value

```csharp
int count = dbContext
    .Sql("SELECT COUNT(*) FROM Product")
    .QuerySingle<int>();
```

### Query for a list of scalar values

```csharp
List<int> ids = dbContext
    .Sql("SELECT ProductId FROM Product")
    .QueryMany<int>();
```

### Parameters

#### Indexed parameters

```csharp
dynamic products = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = @0 OR ProductId = @1", 1, 2)
    .QueryMany<dynamic>();
```

Or:

```csharp
dynamic products = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = @0 OR ProductId = @1")
    .Parameters(1, 2)
    .QueryMany<dynamic>();
```

#### Named parameters

```csharp
dynamic products = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = @Id1 OR ProductId = @Id2")
    .Parameter("Id1", 1)
    .Parameter("Id2", 2)
    .QueryMany<dynamic>();
```

#### Output parameter

```csharp
var cmd = dbContext
    .Sql("SELECT @ProductName = Name FROM Product WHERE ProductId = 1")
    .ParameterOut("ProductName", DataTypes.String, 100);

cmd.Execute();
string productName = cmd.ParameterValue<string>("ProductName");
```

#### List of parameters - `IN` operator

```csharp
List<int> ids = new() { 1, 2, 3, 4 };

// Note: Do not leave whitespace around IN(...) syntax
dynamic products = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId IN(@0)", ids)
    .QueryMany<dynamic>();
```

#### `LIKE` operator

```csharp
string pattern = "%abc%";
dbContext.Sql("SELECT * FROM Product WHERE ProductName LIKE @0", pattern);
```

### Mapping

#### Automapping - 1:1 match

```csharp
List<Product> products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<Product>();
```

#### Automap to a custom collection

```csharp
ProductionCollection products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<Product, ProductionCollection>();
```

#### Automapping - Mismatch resolution using SQL alias

```csharp
List<Product> products = dbContext
    .Sql(@"SELECT p.*,
        c.CategoryId AS Category_CategoryId,
        c.Name AS Category_Name
        FROM Product p
        INNER JOIN Category c ON p.CategoryId = c.CategoryId")
    .QueryMany<Product>();
```

Here:

- `p.*` (ProductId, Name) maps to `Product.ProductId` and `Product.Name`
- `Category_CategoryId` maps to `Product.Category.CategoryId`
- `Category_Name` maps to `Product.Category.Name`

#### Custom mapping using dynamic

```csharp
List<Product> products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<Product>(CustomMapper);

public void CustomMapper(Product product, dynamic row)
{
    product.ProductId = row.ProductId;
    product.Name = row.Name;
}
```

#### Custom mapping using IDataReader

```csharp
List<Product> products = dbContext
    .Sql("SELECT * FROM Product")
    .QueryMany<Product>(CustomMapper);

public void CustomMapper(Product product, IDataReader row)
{
    product.ProductId = row.GetInt32("ProductId");
    product.Name = row.GetString("Name");
}
```

#### Complex entity mapping

For complex entities requiring custom creation logic:

```csharp
var products = new List<Product>();

dbContext.Sql("SELECT * FROM Product")
    .QueryComplexMany(products, MapComplexProduct);

private void MapComplexProduct(IList<Product> products, IDataReader reader)
{
    products.Add(new Product
    {
        ProductId = reader.GetInt32("ProductId"),
        Name = reader.GetString("Name")
    });
}
```

### Multiple result sets

FluentData.Core supports multiple result sets, allowing multiple queries in a single database call. Wrap the code in a `using` statement to ensure the connection closes properly.

```csharp
using (var cmd = dbContext.MultiResultSql)
{
    List<Category> categories = cmd
        .Sql("SELECT * FROM Category; SELECT * FROM Product;")
        .QueryMany<Category>();

    // Reuses data from the first query
    List<Product> products = cmd.QueryMany<Product>();
}
```

The first `QueryMany` executes against the database. Subsequent calls reuse the data from the first execution.

### Select data and Paging

The Select builder simplifies data retrieval and paging:

```csharp
List<Product> products = dbContext
    .Select<Product>("p.*, c.Name AS Category_Name")
    .From("Product p INNER JOIN Category c ON c.CategoryId = p.CategoryId")
    .Where("p.ProductId > 0 AND p.Name IS NOT NULL")
    .OrderBy("p.Name")
    .Paging(1, 10)
    .QueryMany();
```

`Paging(1, 10)` returns the first 10 products.

### Insert data

#### Using SQL

```csharp
int id = dbContext
    .Sql("INSERT INTO Product(Name, CategoryId) VALUES(@0, @1)")
    .Parameters("The Warren Buffet Way", 1)
    .ExecuteReturnLastId<int>();
```

#### Using a builder

```csharp
int id = dbContext
    .Insert("Product")
    .Column("Name", "The Warren Buffet Way")
    .Column("CategoryId", 1)
    .ExecuteReturnLastId<int>();
```

#### Using a builder with automapping

```csharp
var product = new Product
{
    Name = "The Warren Buffet Way",
    CategoryId = 1
};

// Exclude ProductId (identity column)
int id = dbContext
    .Insert<Product>("Product", product)
    .AutoMap(x => x.ProductId)
    .ExecuteReturnLastId<int>();

// If table name matches class name
int id = dbContext
    .Insert<Product>(product)
    .AutoMap(x => x.ProductId)
    .ExecuteReturnLastId<int>();
```

Pass identity columns to `AutoMap` to exclude them, as their values are generated by the database.

### Update data

#### Using SQL

```csharp
int rows = dbContext
    .Sql("UPDATE Product SET Name = @0 WHERE ProductId = @1")
    .Parameters("The Warren Buffet Way", 1)
    .Execute();
```

#### Using a builder

```csharp
int rows = dbContext
    .Update("Product")
    .Column("Name", "The Warren Buffet Way")
    .Where("ProductId", 1)
    .Execute();
```

#### Using a builder with automapping

```csharp
var product = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();

product.Name = "The Warren Buffet Way";

int rows = dbContext
    .Update<Product>("Product", product)
    .AutoMap(x => x.ProductId)
    .Where(x => x.ProductId)
    .Execute();
```

Pass identity columns to `AutoMap` to prevent them from being updated.

##### Insert and update - common Fill method

```csharp
var product = new Product();
product.Name = "The Warren Buffet Way";
product.CategoryId = 1;

dbContext.Insert<Product>("Product", product)
    .Fill(FillBuilder)
    .Execute();



dbContext.Update<Product>("Product", product)
    .Fill(FillBuilder)
    .Execute();

public void FillBuilder(IInsertUpdateBuilder<Product> builder)
{
    builder.Column(x => x.Name);
    builder.Column(x => x.CategoryId);
}
```

### Delete data

#### Using SQL

```csharp
int rows = dbContext
    .Sql("DELETE FROM Product WHERE ProductId = 1")
    .Execute();
```

#### Using a builder

```csharp
int rows = dbContext
    .Delete("Product")
    .Where("ProductId", 1)
    .Execute();
```

### Stored procedure

#### Using SQL

```csharp
var rows = dbContext
    .Sql("ProductUpdate")
    .CommandType(DbCommandTypes.StoredProcedure)
    .Parameter("ProductId", 1)
    .Parameter("Name", "The Warren Buffet Way")
    .Execute();
```

#### Using a builder

```csharp
var rows = dbContext
    .StoredProcedure("ProductUpdate")
    .Parameter("Name", "The Warren Buffet Way")
    .Parameter("ProductId", 1)
    .Execute();
```

#### Using a builder with automapping

```csharp
var product = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();

product.Name = "The Warren Buffet Way";

var rows = dbContext
    .StoredProcedure<Product>("ProductUpdate", product)
    .AutoMap(x => x.CategoryId)
    .Execute();
```

#### Using a builder with expressions

```csharp
var product = dbContext
    .Sql("SELECT * FROM Product WHERE ProductId = 1")
    .QuerySingle<Product>();

product.Name = "The Warren Buffet Way";

var rows = dbContext
    .StoredProcedure<Product>("ProductUpdate", product)
    .Parameter(x => x.ProductId)
    .Parameter(x => x.Name)
    .Execute();
```

### Transactions

FluentData.Core supports transactions. Wrap the code in a `using` statement to ensure the connection closes. If an exception occurs or `Commit` is not called, the transaction automatically rolls back.

```csharp
using (var ctx = dbContext.UseTransaction(true))
{
    ctx.Sql("UPDATE Product SET Name = @0 WHERE ProductId = @1")
        .Parameters("The Warren Buffet Way", 1)
        .Execute();

    ctx.Sql("UPDATE Product SET Name = @0 WHERE ProductId = @1")
        .Parameters("Bill Gates Bio", 2)
        .Execute();

    ctx.Commit();
}
```

### Entity factory

The entity factory creates object instances during automapping. For complex business objects requiring special creation logic, create a custom entity factory:

```csharp
List<Product> products = dbContext
    .EntityFactory(new CustomEntityFactory())
    .Sql("SELECT * FROM Product")
    .QueryMany<Product>();

public class CustomEntityFactory : IEntityFactory
{
    public object Create(Type type) => Activator.CreateInstance(type);
}
```

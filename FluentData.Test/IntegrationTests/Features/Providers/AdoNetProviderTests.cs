using FluentData.Core;
using FluentData.Test.IntegrationTests.Models;
using System.Data.Common;
using System.Data.SqlClient;

namespace FluentData.Test.IntegrationTests.Features.Providers
{

	public class AdoNetProviderTests
	{
		
		public void Test1()
		{
			var context = new DbContext().ConnectionString(TestHelper.GetConnectionStringValue("SqlServer"), new SqlServerProvider(), new CustomDbProviderFactory());
			context.Sql("select * from Product where ProductId = 1").QuerySingle<Product>();
		}

		
		public void Test2()
		{
			new DbContext().ConnectionStringName("ProviderTest", new SqlServerProvider());
		}

		
		public void Test3()
		{
			new DbContext().ConnectionStringName("ProviderTest2", new SqlServerProvider());
		}
	}

	public class CustomDbProviderFactory : System.Data.Common.DbProviderFactory
	{
		public override DbConnection CreateConnection()
		{
			return new SqlConnection();
		}
	}
}

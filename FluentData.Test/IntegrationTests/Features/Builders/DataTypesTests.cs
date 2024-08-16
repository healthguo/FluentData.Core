using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Features.Builders
{

	public class DataTypesTests : BaseSqlServerIntegrationTest
	{
		
		public void Update_values()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValue();
				value.DecimalValue = 5;
				value.StringValue = "test";
				value.DateTimeValue = DateTime.Now;
				value.FloatValue = 12.12F;

				value.Id = context.Insert("DataTypeValue")
							.Column("DecimalValue", value.DecimalValue)
							.Column("StringValue", value.StringValue)
							.Column("DateTimeValue", value.DateTimeValue)
							.Column("FloatValue", value.FloatValue)
							.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue")
						.Column("DecimalValue", value.DecimalValue)
						.Column("StringValue", value.StringValue)
						.Column("DateTimeValue", value.DateTimeValue)
						.Column("FloatValue", value.FloatValue)
						.Where("Id", value.Id)
						.Execute();
			}
		}

		
		public void Update_values_not_nullable()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValueNotNullable();
				value.DecimalValue = 5;
				value.StringValue = "test";
				value.DateTimeValue = DateTime.Now;
				value.FloatValue = 12.12F;

				value.Id = context.Insert("DataTypeValue")
							.Column("DecimalValue", value.DecimalValue)
							.Column("StringValue", value.StringValue)
							.Column("DateTimeValue", value.DateTimeValue)
							.Column("FloatValue", value.FloatValue)
							.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue")
					.Column("DecimalValue", value.DecimalValue)
					.Column("StringValue", value.StringValue)
					.Column("DateTimeValue", value.DateTimeValue)
					.Column("FloatValue", value.FloatValue)
					.Where("Id", value.Id)
					.Execute();
			}
		}

		
		public void Update_values_expression()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValue();
				value.DecimalValue = 5;
				value.StringValue = "test";
				value.DateTimeValue = DateTime.Now;
				value.FloatValue = 12.12F;

				value.Id = context.Insert("DataTypeValue", value)
							.Column(x => x.DecimalValue)
							.Column(x => x.StringValue)
							.Column(x => x.DateTimeValue)
							.Column("FloatValue", value.FloatValue)
							.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue", value)
					.Column(x => x.DecimalValue)
					.Column(x => x.StringValue)
					.Column(x => x.DateTimeValue)
					.Column("FloatValue", value.FloatValue)
					.Where(x => x.Id)
					.Execute();
			}
		}

		
		public void Update_values_automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValue();
				value.DecimalValue = 5;
				value.StringValue = "test";
				value.DateTimeValue = DateTime.Now;
				value.FloatValue = 12.12F;

				value.Id = context.Insert("DataTypeValue", value)
								.AutoMap(x => x.Id)
								.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue", value)
					.AutoMap(x => x.Id)
					.Where(x => x.Id)
					.Execute();
			}
		}

		
		public void Update_null_values()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValue();
				value.DecimalValue = null;
				value.StringValue = null;
				value.DateTimeValue = null;
				value.FloatValue = null;

				value.Id = context.Insert("DataTypeValue")
							.Column("DecimalValue", value.DecimalValue)
							.Column("StringValue", value.StringValue)
							.Column("DateTimeValue", value.DateTimeValue)
							.Column("FloatValue", value.FloatValue)
							.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue")
					.Column("DecimalValue", value.DecimalValue)
					.Column("StringValue", value.StringValue)
					.Column("DateTimeValue", value.DateTimeValue)
					.Column("FloatValue", value.FloatValue)
					.Where("Id", value.Id)
					.Execute();
			}
		}

		
		public void Update_null_expression()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValue();
				value.DecimalValue = null;
				value.StringValue = null;
				value.DateTimeValue = null;
				value.FloatValue = null;				

				value.Id = context.Insert("DataTypeValue", value)
							.Column(x => x.DecimalValue)
							.Column(x => x.StringValue)
							.Column(x => x.DateTimeValue)
							.Column("FloatValue", value.FloatValue)
							.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue", value)
					.Column(x => x.DecimalValue)
					.Column(x => x.StringValue)
					.Column(x => x.DateTimeValue)
					.Column("FloatValue", value.FloatValue)
					.Where("Id", value.Id)
					.Execute();
			}
		}

		
		public void Update_null_automap()
		{
			using (var context = Context.UseTransaction(true))
			{
				var value = new DataTypeValue();
				value.DecimalValue = null;
				value.StringValue = null;
				value.DateTimeValue = null;
				value.FloatValue = null;

				value.Id = context.Insert("DataTypeValue", value)
							.AutoMap(x => x.Id)
							.ExecuteReturnLastId<int>();

				context.Update("DataTypeValue", value)
					.AutoMap(x => x.Id)
					.Where(x => x.Id)
					.Execute();
			}
		}


		
		public void ByteArrayTest()
		{
			using(var context = Context.UseTransaction(true))
			{
				context.Insert("DataTypeValue")
					.Column("VarBinaryValue", new byte[10000])
					.Execute();
			}
		}
	}
}

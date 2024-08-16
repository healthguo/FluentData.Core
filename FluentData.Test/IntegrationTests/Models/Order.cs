using System;

namespace FluentData.Test.IntegrationTests.Models
{
	public class Order
	{
		public int OrderId { get; set; }
		public Product Product { get; set; }
		public DateTime Created { get; set; }
	}
}

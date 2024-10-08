﻿using System;

namespace FluentData.Test.IntegrationTests.Models
{
	public class OrderReport
	{
		public int OrderId { get; set; }
		public DateTime Created { get; set; }
		public OrderLine OrderLine { get; set; }
	}

	public class OrderLine
	{
		public int OrderLineId { get; set; }
		public Product Product { get; set; }
	}
}

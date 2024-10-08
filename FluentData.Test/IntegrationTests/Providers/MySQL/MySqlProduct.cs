﻿using FluentData.Test.IntegrationTests.Models;

namespace FluentData.Test.IntegrationTests.Providers.MySql
{
	public class MySqlProduct
	{
		public int ParamProductId { get; set; }
		public string ParamName { get; set; }
		public int ParamCategoryId { get; set; }

		public MySqlProduct(Product product)
		{
			ParamProductId = product.ProductId;
			ParamName = product.Name;
			ParamCategoryId = product.CategoryId;
		}
	}
}

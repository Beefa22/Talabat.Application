using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
	{
		public static async Task SeedAsync(StoreContext dbContext)
		{
			if (!dbContext.ProductBrands.Any())//Check if ProductBrand Set don't have any product to seed data (cuz we seed data only once)
			{
				var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");//will store here as string
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

				if (brands is not null && brands.Count > 0)
				{
					foreach (var brand in brands)
						await dbContext.Set<ProductBrand>().AddAsync(brand);

					await dbContext.SaveChangesAsync();
				}
			}
			if (!dbContext.ProductTypes.Any())
			{
				var productType = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
				var Types = JsonSerializer.Deserialize<List<ProductType>>(productType);
				if (Types?.Count > 0)
				{
					foreach (var type in Types)
						await dbContext.Set<ProductType>().AddAsync(type);

					await dbContext.SaveChangesAsync();
				}

			}
			if (!dbContext.Products.Any())
			{
				var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);
				if (products is not null && products.Count > 0)
				{
					foreach (var product in products)
						await dbContext.Set<Product>().AddAsync(product);
					await dbContext.SaveChangesAsync();
				}
			}
			if (!dbContext.DeliveryMethods.Any())
			{
				var deliveryMethod = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
				var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethod);
				if (methods is not null && methods.Count > 0)
				{
					foreach (var method in methods)
						await dbContext.Set<DeliveryMethod>().AddAsync(method);
					await dbContext.SaveChangesAsync();
				}
			}


		}
	}
}

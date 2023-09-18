using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Config;

namespace Talabat.Repository.Data
{
    public class StoreContext:DbContext
	{
		public StoreContext(DbContextOptions<StoreContext> options):base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//Fluent APIs
			//modelBuilder.ApplyConfiguration(new ProductConfiguration());
			//modelBuilder.ApplyConfiguration(new ProductBrandConfiguration());
			//modelBuilder.ApplyConfiguration(new ProductTypeConfiguration());
			
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());// this line is replacement of earlier three lines
																						  // Search about Reflection in .Net

		}

		public DbSet<Product> Products { get; set; }
		public DbSet<ProductBrand> ProductBrands { get; set; }
		public DbSet<ProductType> ProductTypes { get; set; }
	}
}

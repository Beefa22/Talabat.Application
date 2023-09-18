using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.HasOne(P => P.ProductBrand).WithMany(/*B=>B.Products*/)//we didn't create navigational property of many in ProductBrand
					.HasForeignKey(P => P.ProductBrandId);//this line is not appropriat i already create it in Product model(just to make sure)

			builder.HasOne(P => P.ProductType).WithMany();

			//DataAnnotation prefered in Fluent APIs

			builder.Property(P => P.Name).IsRequired().HasMaxLength(100);
			builder.Property(P => P.Description).IsRequired();
			builder.Property(P => P.PictureUrl).IsRequired();
			builder.Property(P => P.Price).HasColumnType("decimal(18,2)");//To avoid warning in migration 
		}
	} 
}

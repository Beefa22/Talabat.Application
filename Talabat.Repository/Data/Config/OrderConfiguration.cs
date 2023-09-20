using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(O => O.ShippingAddress, shippingAddress => shippingAddress.WithOwner());
			builder.Property(O => O.Status)
				.HasConversion(
				Ostatus => Ostatus.ToString(),//String Type in DataBase
				Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)//Returned type from DataBase
				);
			builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
		}
	}
}

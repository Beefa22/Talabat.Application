using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Order:BaseEntity
	{
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Address ShippingAddress { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }//Navigational Pro [one]
		public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();//Navigational Prop [Many]
		public decimal SubTotal { get; set; }
		public decimal GetTotal()// Driven Attribute
			=> SubTotal + DeliveryMethod.Cost;

	}
}

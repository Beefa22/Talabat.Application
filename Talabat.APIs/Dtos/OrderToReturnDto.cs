using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
	public class OrderToReturnDto
	{
		public int Id { get; set; }
		[Required]
		public string BuyerEmail { get; set; }
		public DateTimeOffset OrderDate { get; set; } 
		public string Status { get; set; } 
		public Address ShippingAddress { get; set; }
		//public DeliveryMethod DeliveryMethod { get; set; }
		public string DeliveryMethod { get; set; }
		public decimal DeliveryMethodCost { get; set; }
		public ICollection<OrderItemDto> Items { get; set; } 
		public decimal SubTotal { get; set; }
		public string PaymentIntentId { get; set; }
		public decimal Total { get; set; } 
	}
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class OrderDto
	{
		[Required]
		public string BasketId { get; set; }
		//[Required]// already required cuz of int
		public int DeliveryMethodId { get; set; }
		public AddressDto Address { get; set; }
	}
}

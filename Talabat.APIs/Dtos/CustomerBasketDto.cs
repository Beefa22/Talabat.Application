using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
	public class CustomerBasketDto
	{
		public string Id { get; set; }
		public List<BasketItemsDto> Items { get; set; }
	}
}

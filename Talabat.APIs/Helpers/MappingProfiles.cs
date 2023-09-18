using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.ProductBrand, O => O.MapFrom(s => s.ProductBrand.Name))//Configuration
				.ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
				.ForMember(d => d.PictureUrl, O => O.MapFrom<PictureUrlResolver>());
		
			CreateMap<Address, AddressDto>().ReverseMap();

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemsDto, BasketItems>();
		}
	}
}

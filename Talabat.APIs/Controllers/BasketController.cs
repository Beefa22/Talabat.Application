﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}
		[HttpGet]//Api/Basket?id
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)//this endPoint is for get and Recreate if the basket Expired
		{
			var basket = await _basketRepository.GetBasketAsync(id);
			return basket is null ? new CustomerBasket(id) : basket;
		}

		[HttpPost]
		public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
		{
			var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

			var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
			if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
			return Ok(createdOrUpdatedBasket);
		}

		[HttpDelete]
		public async Task<ActionResult<bool>> DeleteBasket(string id)
		{
			return await _basketRepository.DeleteBasketAsync(id);
		}

	}
}

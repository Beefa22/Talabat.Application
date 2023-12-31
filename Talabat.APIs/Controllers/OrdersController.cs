﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class OrdersController : BaseController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		

		public OrdersController(IOrderService orderService,
								IMapper mapper
			)
		{
			_orderService = orderService;
			_mapper = mapper;
	
		}

		[ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]

		[HttpPost]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var address = _mapper.Map<AddressDto, Address>(orderDto.Address);

		var order = await _orderService.CreateOrderAsync(buyerEmail,orderDto.BasketId,orderDto.DeliveryMethodId,address);

			if (order is null) return BadRequest(new ApiResponse(400));

			return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
		}

		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var orders = await _orderService.GetOrderForUserAsync(buyerEmail);

			if (orders is null) return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
		}

		
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{orderId}")]
		public async Task<ActionResult<Order>> GetOrderForUser(int orderId)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var order = await _orderService.GetOrderByIdForUserAsync(buyerEmail,orderId);

			if (order is null) return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
		}

		[HttpGet("deliverymethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
		{
			var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

			return Ok(deliveryMethods);
		}
	}
}

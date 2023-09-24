using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class PaymentsController : BaseController
	{
		private readonly IPaymentService _paymentService;

		public PaymentsController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}
		[HttpPost("{basketId}")] //Post : Api/Payments/basketId
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null) return BadRequest(new ApiResponse(400, "There is a Problem with your Basket!"));
			return Ok(basket);
		}
	}
}

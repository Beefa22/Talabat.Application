using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.Services;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
	public class PaymentSerivce : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBasketRepository _basketRepo;

		public PaymentSerivce(IConfiguration configuration,
							IUnitOfWork unitOfWork,
							IBasketRepository basket
														)
		{
			_configuration = configuration;
			_unitOfWork = unitOfWork;
			_basketRepo = basket;
		}
		public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
		{
			StripeConfiguration.ApiKey = _configuration["StripeSetting:SecretKey"];

			var basket = await _basketRepo.GetBasketAsync(basketId);
			if (basket is null) return null;

			var shippingPrice = 0m;
			if (basket.DeliveryMethodId.HasValue)	
			{
			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
				basket.ShippingCost = deliveryMethod.Cost;
				shippingPrice = deliveryMethod.Cost;
			}

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;
				}
			}

			var service = new PaymentIntentService();
			PaymentIntent paymentIntent;

			if (string.IsNullOrEmpty(basket.PaymentIntentId))//Create PaymentIntent
			{
				var options = new PaymentIntentCreateOptions()
				{
					Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};
				paymentIntent = await service.CreateAsync(options);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else //Update PaymentIntent
			{
				var options = new PaymentIntentUpdateOptions()
				{
				Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100
				};
				await service.UpdateAsync(basket.PaymentIntentId,options);
			}

			await _basketRepo.UpdateBasketAsync(basket);
			return basket;
		}
	}
}

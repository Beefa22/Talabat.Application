using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Interfaces;
using Talabat.Core.IRepositories;
using Talabat.Core.Services;
using Talabat.Core.Specification;

namespace Talabat.Service
{
	public class OrderServices : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		////private readonly IGenericRepository<Product> _productRepo;
		////private readonly IGenericRepository<DeliveryMethod> _deliveryMethod;
		////private readonly IGenericRepository<Order> _orderRepo;

		public OrderServices(
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork,
			IPaymentService paymentService
			///IGenericRepository<Product> productRepo,
			///IGenericRepository<DeliveryMethod> deliveryMethod,
			///IGenericRepository<Order> orderRepo
			)
		{
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
			_basketRepo = basketRepo;

			///_productRepo = productRepo;
			///_deliveryMethod = deliveryMethod;
			///_orderRepo = orderRepo;
		}
		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			//Get BasketItems form BasketRepo
			var basket = await _basketRepo.GetBasketAsync(basketId);
			//Get selected items in basket from ProdcutRepo

			var orderItems = new List<OrderItem>();

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

					var productItemsOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

					var orderItem = new OrderItem(productItemsOrdered,product.Price,item.Quantity);

					orderItems.Add(orderItem);
				
				}

			}

			//Calculate SubTotal
			var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

			//Get Delivery method from deliveryMethodRepo
			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

			//Create Order
			var spec = new OrderWithPaymentIntentIdSpecification(basket.PaymentIntentId);

			var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
			if (existingOrder is not null)
			{
				 _unitOfWork.Repository<Order>().Delete(existingOrder);
				await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			}
				
			var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);
			await _unitOfWork.Repository<Order>().Add(order);

			//Save Changes in DataBase
			var result = await _unitOfWork.Complete();
			if (result <= 0) return null;
			return order;

		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
			return deliveryMethods;
		}

		public async Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			var spec = new OrderSpecification(buyerEmail,orderId);

			var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
			
			return order;
		}

		public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
		{
			var spec = new OrderSpecification(buyerEmail);

			var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
			return orders;
		}
	}
}

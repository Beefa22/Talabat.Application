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

namespace Talabat.Service
{
	public class OrderServices : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;

		////private readonly IGenericRepository<Product> _productRepo;
		////private readonly IGenericRepository<DeliveryMethod> _deliveryMethod;
		////private readonly IGenericRepository<Order> _orderRepo;

		public OrderServices(
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork

			///IGenericRepository<Product> productRepo,
			///IGenericRepository<DeliveryMethod> deliveryMethod,
			///IGenericRepository<Order> orderRepo
			)
		{
			_unitOfWork = unitOfWork;
			///_basketRepo = basketRepo;
			///_productRepo = productRepo;
			///_deliveryMethod = deliveryMethod;
			///_orderRepo = orderRepo;
		}
		public async Task<Order?> CreateOrder(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
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
			var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal);
			await _unitOfWork.Repository<Order>().Add(order);

			//Save Changes in DataBase
			var result = await _unitOfWork.Complete();
			if (result <= 0) return null;
			return order;

		}

		public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}

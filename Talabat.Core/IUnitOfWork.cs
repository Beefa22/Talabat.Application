using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Interfaces;

namespace Talabat.Core
{
	public interface IUnitOfWork:IAsyncDisposable
	{
		////public IGenericRepository<Product> ProductRepo { get; set; }
		////public IGenericRepository<ProductBrand> BrandRepo { get; set; }
		////public IGenericRepository<ProductType> TypeRepo { get; set; }
		////public IGenericRepository<Order> OrderRepo { get; set; }
		////public IGenericRepository<OrderItem> OrderItemRepo { get; set; }
		////public IGenericRepository<DeliveryMethod> DeliveryMethodRepo { get; set; }

		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
		
		Task<int> Complete();
	}
}

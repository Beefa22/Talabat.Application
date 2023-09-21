using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Interfaces;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;
		private Hashtable _repositories;

		public UnitOfWork(StoreContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
			
			///against SOLID Princples
			///ProductRepo = new GenericRepository<Product>(_dbContext);
			///BrandRepo = new GenericRepository<ProductBrand>(_dbContext);
			///TypeRepo = new GenericRepository<ProductType>(_dbContext);
			///OrderRepo = new GenericRepository<Order>(_dbContext);
			///OrderItemRepo = new GenericRepository<OrderItem>(_dbContext);
			///DeliveryMethodRepo = new GenericRepository<DeliveryMethod>(_dbContext);
			

		}
		////public IGenericRepository<Product> ProductRepo { get; set; }
		////public IGenericRepository<ProductBrand> BrandRepo { get; set; }
		////public IGenericRepository<ProductType> TypeRepo { get; set; }
		////public IGenericRepository<Order> OrderRepo { get; set; }
		////public IGenericRepository<OrderItem> OrderItemRepo { get; set; }
		////public IGenericRepository<DeliveryMethod> DeliveryMethodRepo { get; set; }

		public async Task<int> Complete()
		=> await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
		=> await _dbContext.DisposeAsync();

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var type = typeof(TEntity).Name;

			if (!_repositories.ContainsKey(type))
			{
				var repository = new GenericRepository<TEntity>(_dbContext);

				_repositories.Add(type, repository);
			}
			
			return _repositories[type] as IGenericRepository<TEntity>;
		}
	}
}

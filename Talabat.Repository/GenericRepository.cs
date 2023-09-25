using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specification;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbcontext;

		public GenericRepository(StoreContext dbcontext)
		{
			_dbcontext = dbcontext;
		}
		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			//if (typeof(T) == typeof(Product))
			//	return (IEnumerable<T>) await _dbcontext.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();
			
			return await _dbcontext.Set<T>().ToListAsync();
		}


		public async Task<T> GetByIdAsync(int id)
		{
	//await _dbcontext.Products.Where(x=>x.Id==id).Include(P => P.ProductBrand).Include(P => P.ProductType).FirstorDefault();
			return await _dbcontext.Set<T>().FindAsync(id);
		}

		
		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISepcification<T> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public async Task<T> GetEntityWithSpecAsync(ISepcification<T> spec)
		{
			return await ApplySpecification(spec).FirstOrDefaultAsync();
		}

		public async Task<int> GetCountWithSpecAsync(ISepcification<T> spec)
		{
			return await ApplySpecification(spec).CountAsync();
		}
		private IQueryable<T> ApplySpecification(ISepcification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
		}

		public async Task Add(T entity)
		=> await _dbcontext.Set<T>().AddAsync(entity);

		public void update(T entity)
		=> _dbcontext.Set<T>().Update(entity);

		public void Delete(T entity)
		=> _dbcontext.Set<T>().Remove(entity);
	}
}

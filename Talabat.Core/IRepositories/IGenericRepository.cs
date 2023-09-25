using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
	{
		//Static signiture 
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task Add(T entity);
		void update(T entity);
		void Delete(T entity);

		//Dynamic signiture
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISepcification<T> spec);
		Task<T> GetEntityWithSpecAsync(ISepcification<T> spec);

		Task<int> GetCountWithSpecAsync(ISepcification<T> spec);

	}
}

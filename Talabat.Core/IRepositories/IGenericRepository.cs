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

		//Dynamic signiture
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISepcification<T> spec);
		Task<T> GetByIdWithSpecAsync(ISepcification<T> spec);

		Task<int> GetCountWithSpecAsync(ISepcification<T> spec);

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.IRepositories
{
    public interface IBasketRepository
	{
		Task<CustomerBasket?> GetBasketAsync(string basketid);
		Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);//Will work as Create too.
		Task<bool> DeleteBasketAsync(string	 basketId);
	}
}

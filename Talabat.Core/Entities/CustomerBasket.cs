using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }//String cuz of Guid
        public List<BasketItems> Items { get; set; }

        public CustomerBasket(string id)
        {
            Id = id;
        }
    }
}

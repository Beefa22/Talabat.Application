using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
        }
        public string Id { get; set; }//String cuz of Guid
        public List<BasketItems> Items { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingCost { get; set; }

    }
}

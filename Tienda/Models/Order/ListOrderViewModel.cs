using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda.Models.Order
{
    public class ListOrderViewModel
    {
        public OrderViewModel Order { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

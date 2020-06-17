using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda.Models.Order
{
    public class ItemViewModel
    {
        public string Code { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Guid sessionId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda.Models.Order
{
    public class OrderViewModel
    {
        public string Name { get; set; }

        public ItemViewModel Item { get; set; }

        public List<ItemViewModel> ItemsSelected { get; set; }

        public Guid sessionId { get; set; }
    }
}

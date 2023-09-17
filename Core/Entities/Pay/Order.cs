using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Pay
{
    public class Order:BaseEntity
    {
        public string? Number { get; set; }
        public string? Condition { get; set; }

        public string? Shipping { get; set; }
        public string? Payment { get; set; }
        public int? Subtotal { get; set; }
        public int? Freight { get; set; }
        public int Total { get; set; }
        public Recipient? Recipient { get; set; }
        public List<Item> List { get; set; }
    }
}
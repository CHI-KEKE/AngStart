using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Pay
{
    public class Item:BaseEntity
    {
        public int? Product_Id { get; set; }

        public string? Name{get;set;}
        public int Price { get; set; }
        public Color Color { get; set; }
        public string Size { get; set; }
        public int Qty { get; set; }
    }
}
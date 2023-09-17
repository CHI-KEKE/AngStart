using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductData
    {
        public int CountAfterFiltering { get; set; }
        public List<Product>? CurrentPageProducts { get; set; }
        public decimal TotalPageNumber { get; set; }
    }
}
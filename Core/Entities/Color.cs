using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Color:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        // public List<Product>? Products {get;set;}
    }
}
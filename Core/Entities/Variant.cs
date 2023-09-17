using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Variant:BaseEntity
    {
    public string? ColorCode { get; set; }
    public string? Size { get; set; }
    public int? Stock { get; set; }

    // Add a reference to the corresponding Color entity
    // public Color? Color { get; set; }  

    // public List<Product>? Products {get;set;}

    }
}
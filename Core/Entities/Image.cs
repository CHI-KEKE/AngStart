using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Image:BaseEntity
    {
        public string Url { get; set; }
        
        // [ForeignKey("ProductId")]
        // public Product? Product { get; set; }
    }
}
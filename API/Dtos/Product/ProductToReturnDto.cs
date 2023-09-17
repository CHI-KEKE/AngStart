using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Product
{
    public class ProductToReturnDto
    {
            public long Id { get; set; }
            public string Category { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int? Price { get; set; }

            public string Texture { get; set; }

            public string Wash { get; set; }

            public string Place { get; set; }

            public string Note { get; set; }

            public string Story { get; set; }

            public string? MainImage { get; set; }

            public List<string> Images { get; set; }
            public List<string> Colors { get; set; }
            public List<string> Sizes { get; set; }
            public List<string> Stocks { get; set; }         
    }
}
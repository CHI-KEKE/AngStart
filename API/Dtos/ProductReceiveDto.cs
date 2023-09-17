using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductReceiveDto
    {
    public string Category { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? Price { get; set; }
    public string? Texture { get; set; }
    public string? Wash { get; set; }
    public string? Place { get; set; }
    public string? Note { get; set; }
    public string? Story { get; set; }
    public IFormFile MainImage { get; set; }
    public List<IFormFile> Images { get; set; }
    public List<string> VariantColors { get; set; }
    public List<string> VariantColorNames { get; set; }
    public List<string> VariantSizes { get; set; }
    public List<int> VariantStocks { get; set; }


    }
}
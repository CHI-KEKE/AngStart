using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Orders
{
    public class OrderDataDto
    {
        public int Total { get; set; }
        public List<ItemDataDto> List { get; set; }       
    }

    public class ItemDataDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public ColorDataDto Color { get; set; }
        public string Size { get; set; }
        public int Qty { get; set; }
    }

    public class ColorDataDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
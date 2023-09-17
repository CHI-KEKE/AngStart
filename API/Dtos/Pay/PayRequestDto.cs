using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Pay
{
    public class PayRequestDto
    {
        public string Prime { get; set; }
        public OrderDto Order { get; set; }
    }
    public class ColorDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ColorDto Color { get; set; }
        public string Size { get; set; }
        public int Qty { get; set; }
    }

    public class OrderDto
    {
        public string Shipping { get; set; }
        public string Payment { get; set; }
        public int Subtotal { get; set; }
        public int Freight { get; set; }
        public int Total { get; set; }
        public RecipientDto Recipient { get; set; }
        public List<ItemDto> List { get; set; }
    }

    public class RecipientDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Time { get; set; }
        public string Zipcode { get; set; }
        public string Nationalid { get; set; }
    }



}
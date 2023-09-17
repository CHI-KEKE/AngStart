using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Dtos.Pay;
using Core.Entities;
using Core.Entities.Pay;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class PayController :BaseApiController
    {
        private readonly PayService _PayService;

        private readonly StoreContext _context;

        public PayController(StoreContext context)
        {
            _context = context;
            _PayService = new PayService();
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<PayReturnToEFDto> CreatePayment(PayRequestDto dto)
        {
            Random random = new Random();
            int randomNumber = random.Next(100);
            string randomString = randomNumber.ToString();
            //save unpaid order

            var UnPayOrder = new Order
            {
                Number = randomString,
                Condition = "unpaid",
                Shipping = dto.Order.Shipping,
                Payment = dto.Order.Payment,
                Subtotal = dto.Order.Subtotal,
                Freight = dto.Order.Freight,
                Total = dto.Order.Total,
                Recipient = new Recipient
                {
                    Name = dto.Order.Recipient.Name,
                    Phone = dto.Order.Recipient.Phone,
                    Email = dto.Order.Recipient.Email,
                    Address = dto.Order.Recipient.Address,
                    Time = dto.Order.Recipient.Time,
                    Zipcode = dto.Order.Recipient.Zipcode,
                    Nationalid = dto.Order.Recipient.Nationalid              
                },
                List = new List<Item>()
    
            };
            Console.WriteLine("unpaid!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            foreach(var item in dto.Order.List)
            {
                var WaitAddItem = new Item
                {
                    Name = item.Name,
                    Price = item.Price,
                    Color = new Color
                    {
                        Code = item.Color.Name,
                        Name = item.Color.Name
                    },
                    Qty = item.Qty,
                    Size = item.Size
                };

                UnPayOrder.List.Add(WaitAddItem);
            };

            _context.Orders.Add(UnPayOrder);
            _context.SaveChanges();
            var paymentResponse =  await _PayService.SendPaymentRequest(dto);
            if(paymentResponse.Status == 0)
            {
                var existingOrder = _context.Orders.FirstOrDefault(o => o.Number == UnPayOrder.Number);  
                if (existingOrder != null)
                {
                    existingOrder.Condition = "paid";
                    _context.SaveChanges();
                    Console.WriteLine("Update the Order to Paid Successfully, and the status is 0!!");

                }
            Console.WriteLine("Paid!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

               return new PayReturnToEFDto
               {
                    Number = UnPayOrder.Number
               };
            }

            return  new PayReturnToEFDto
            {
                Message = paymentResponse.Msg
            };
        }
    }
}
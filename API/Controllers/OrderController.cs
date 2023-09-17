using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Dtos.Orders;
using API.Helpers;
using AutoMapper;
using Core.Entities.Pay;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class OrderController:BaseApiController
    {

        private readonly StoreContext _context;
        private readonly OrderDataService _orderDataService;
        private IMapper _mapper;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderController(StoreContext context,OrderDataService orderDataService,IMapper mapper,IHubContext<OrderHub> hubContext)
        {
            _context = context;
            _orderDataService = orderDataService;
            _mapper = mapper;
            _hubContext = hubContext;

        }

        [HttpGet("AutomapperTesting")]
        public async Task<ActionResult<OrderDataDto>> AutomapperTesting()
        {
            var orderData =  new OrderDataDto
            {
                Total = 1234,
                List = new List<ItemDataDto>(){
                    new ItemDataDto{
                        Id = 5,
                        Price = 1826,
                        Color = new ColorDataDto { Code = "#00EEEE", Name = "Aqua" },
                        Size = "L",
                        Qty = 2
                    },
                    new ItemDataDto{
                        Id = 2,
                        Price = 50,
                        Color = new ColorDataDto { Code = "#EEE00", Name = "Yellow" },
                        Size = "S",
                        Qty = 3
                    }
                }       
            };

            if(orderData != null)
            {
               var data = _mapper.Map<OrderDataDto,Order>(orderData);  //test mapping

                return Ok(data);
            }

            return NotFound();



        }

        [HttpGet("Remotedata")]
        public async Task<ActionResult<List<OrderDataDto>>> GetOrderDataAsync()
        {
            var orderData =  await _orderDataService.GetOrderData();
            if(orderData != null)
            {
            //    var data = _mapper.Map<List<OrderDataDto>,List<Order>>(orderData);  //test mapping
                var OrderToDB = new List<Order>();
                foreach(var order in orderData)
                {
                    var NewOrder = new Order
                    {
                        Total = order.Total,
                        List = new List<Item>()             
                    };

                    foreach(var itemDataDto in order.List)
                    {
                        var newItem = new Item{
                            Product_Id = itemDataDto.Id,
                            Price = itemDataDto.Price,
                            Color = new Core.Entities.Color {
                                Code = itemDataDto.Color.Code,
                                Name = itemDataDto.Color.Name,
                            },
                            Size = itemDataDto.Size,
                            Qty = itemDataDto.Qty
                        };
                        NewOrder.List.Add(newItem);
        
                    };
                    OrderToDB.Add(NewOrder);

                }
                Console.WriteLine("Mapiing ok!!!!!!!!!!!!!!!1");
                _context.Orders.AddRange(OrderToDB);
                _context.SaveChanges();
                Console.WriteLine("OrderSave ok!!!!!!!!!!!!!!!");

                return Ok(OrderToDB);
            }

            return NotFound();



        }

        [HttpGet("loaddata")]
        public async Task<ActionResult> LoadDataAsync()
        {

//Revenue
            var Revenue = (long) _context.Orders.Sum(item => item.Total);

//ColorData
            var ColorData = new List<ColorCodeCountDto>();

            var colorCounts = _context.Colors
                .GroupBy(color => color.Name)
                .Select(group => new ColorCodeCountDto{ ColorName = group.Key, ColorCount = group.Count(), ColorCode = group.FirstOrDefault().Code })
                .ToList();

            ColorData = colorCounts;


//Price Range
            var Items = _context.Items.ToList();

            List<int> prices = new List<int>();

            foreach (var item in Items)
            {
                for (int i = 0; i < item.Qty; i++)
                {
                    prices.Add(item.Price);
                }
            }

//Size Stacked

            var items = _context.Items.ToList();

            var topProductCounts = items
                .GroupBy(item => item.Product_Id)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList();


            var top5ProductsDividedBySize = new List<TopProductBySizeDto>();


            foreach (var size in items.Select(item => item.Size).Distinct())
            {
                var productIds = new List<int>();
                var counts = new List<int>();

                foreach (var productId in topProductCounts)
                {
                    var sizeCount = items
                        .Where(item => item.Product_Id == productId && item.Size == size)
                        .Count();

                        productIds.Add(productId ?? 0);
                        counts.Add(sizeCount);
                }

                top5ProductsDividedBySize.Add(new TopProductBySizeDto
                {
                    Ids = productIds,
                    Count = counts,
                    Size = size
                });
            }

//Send out!!
            var orderAll = new OrderDataReturnDto
            {
                Revenue = Revenue,
                ColorData = ColorData,
                Prices = prices,
                TopProductBySize = top5ProductsDividedBySize
            };


            // await _hubContext.Clients.All.SendAsync("UpdateOrderData", orderAll);

            return Ok(orderAll);

        }


    }
}
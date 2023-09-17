using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Dtos.Orders;
using API.Provider;
using Core.Entities;

namespace API.Domain
{
    public class OrderDataService
    {
        private readonly string Order_Data_URl = "http://35.75.145.100:1234/api/1.0/order/data";

        private static HttpClient client = new HttpClient();
        private readonly JsonProvider _jsonProvider = new JsonProvider(); 



        public async Task<List<OrderDataDto>> GetOrderData()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Order_Data_URl),
            };

            var response = await client.SendAsync(request);
            var list = _jsonProvider.Deserialize<List<OrderDataDto>>(await response.Content.ReadAsStringAsync());

            return list;
        }
    }
}
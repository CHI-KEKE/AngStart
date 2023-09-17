using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Provider;
using Core.Entities;
using Core.Specification;

namespace API.Domain
{
    public class ProductService
    {
        public static string host_name = "api.appworks-school.tw";
        public static string api_version = "1.0";
        private readonly string Products_Request_URl = $"https://{host_name}/api/{api_version}/products/all";

        private static HttpClient client = new HttpClient();
        private readonly JsonProvider _jsonProvider = new JsonProvider(); 



        public async Task<List<ProductDto>> GetProducts()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Products_Request_URl),
            };

            var response = await client.SendAsync(request);
            var list = _jsonProvider.Deserialize<ProductsDto>(await response.Content.ReadAsStringAsync());

            return list.Data;
        }


        public (int StatusCode,bool IfNextPaging) CheckPages(int realPage, ProductSpecParams productSpecParams)
        {
            if(realPage < productSpecParams.paging+1)
            {
                return (400,false);
            }

            if(realPage == productSpecParams.paging+1)
            {
                return (200, false);
            }

                return (200,true);

        }
    }
}
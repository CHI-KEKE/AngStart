using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Dtos.Pay;
using API.Provider;

namespace API.Domain
{
    public class PayService
    {


        private readonly string Partner_Key = "partner_PHgswvYEk4QY6oy3n8X3CwiQCVQmv91ZcFoD5VrkGFXo8N7BFiLUxzeG";
        private readonly string Merchant_ID = "AppWorksSchool_CTBC";	


        private static HttpClient client;
        private readonly JsonProvider _jsonProvider;

        public PayService()
        {
            client = new HttpClient();
            _jsonProvider = new JsonProvider();
        }

        
        public async Task<PayCreateResponseDto> SendPaymentRequest(PayRequestDto dto)
        {

            int TotalPriceToPay = 0;
            foreach(var item in dto.Order.List)
            {
                TotalPriceToPay += item.Price;
            }

            var PayToTPObj = new PayToTPDto
            {
                Prime = dto.Prime,
                Partner_key = Partner_Key,
                Merchant_id = Merchant_ID,
                Details = "TapPay Test",
                Amount = TotalPriceToPay,
                Cardholder = new Cardholder
                {
                    Phone_number =dto.Order.Recipient.Phone,
                    Name = dto.Order.Recipient.Name,
                    Email = dto.Order.Recipient.Email,
                    Zip_code = dto.Order.Recipient.Zipcode,
                    Address =dto.Order.Recipient.Address,
                    National_id = dto.Order.Recipient.Nationalid,              
                },
                Remember = true,               
            };

            var json = _jsonProvider.Serialize(PayToTPObj);
            var requestUrl = "https://sandbox.tappaysdk.com/tpc/payment/pay-by-prime";

            client.DefaultRequestHeaders.Add("x-api-key",Partner_Key);

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUrl),
                Content = new StringContent(json,Encoding.UTF8,"application/json")
            };

            var response = await client.SendAsync(request);
            var PayResponse = _jsonProvider.Deserialize<PayCreateResponseDto>(await response.Content.ReadAsStringAsync());

            Console.WriteLine(PayResponse);

            return PayResponse;
        }
    }
}
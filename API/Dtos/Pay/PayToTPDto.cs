using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Pay
{
    public class PayToTPDto
    {
        public string Prime { get; set; }
        public string Partner_key { get; set; }
        public string Merchant_id { get; set; }
        public string Details { get; set; }
        public int Amount { get; set; }
        public Cardholder Cardholder { get; set; }
        public bool Remember { get; set; }        
    }

    public class Cardholder
    {
        public string Phone_number { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Zip_code { get; set; }
        public string Address { get; set; }
        public string National_id { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Pay
{
    public class Recipient:BaseEntity
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
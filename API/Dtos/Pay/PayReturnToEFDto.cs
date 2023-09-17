using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Pay
{
    public class PayReturnToEFDto
    {
        public string? Number { get; set; }
        public string? Message { get; set; }
    }
}
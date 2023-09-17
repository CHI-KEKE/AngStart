using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public string Details { get; set;}
        public ApiException(int statusCode,string details = null, string message = null) : base(statusCode, message)
        {
            this.Details = details;
        }
    }
}
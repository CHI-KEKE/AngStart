using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message{get;set;}
        public object ReturnData { get; set; }
        public ApiResponse(int statusCode, string message = null,object returnData = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            ReturnData = returnData;
        }

        public string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You have made a bad bad request",
                401 => "You are not Authorized, pls get a token or something!",
                404 => "The Resource is not found",
                500 => "API may have something wrong, Errors may lead to despair which lead to career change",
                _ => null,
            };
        }


    }
}
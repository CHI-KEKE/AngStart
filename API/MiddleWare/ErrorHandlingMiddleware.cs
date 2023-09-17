using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.MiddleWare
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception or perform any desired error handling
                Console.WriteLine($"An error occurred: {ex}");

                // Set the response status code and content
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("An error occurred. Please try again later.");
            }
        }
    }
}
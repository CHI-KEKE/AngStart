using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {

        private readonly int _timeToLiveSeconds;

        public CacheAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);
            Console.WriteLine("Have established a cacheKey !!!!!!!!!!!!!!!!!!!!!!!!!!! " + cacheKey);
            Console.WriteLine("Have generate a cacheResponse !!!!!!!!!!!!!!!!!!!!!!!!!!! " + cacheResponse);
            Console.WriteLine("Have found a cacheService !!!!!!!!!!!!!!!!!!!!!!!!!!! " + cacheService);

            if(!string.IsNullOrEmpty(cacheResponse))
            {
                Console.WriteLine("Found the cachedata!!!!!!!!!");
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            Console.WriteLine("ok~ no cacheresponse, let you continue~~~~");
            Console.WriteLine(executedContext + "&&" + executedContext.Result);

            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                Console.WriteLine("i am deemed as okobject successfully!!!!!!!!!!!!!!!!!!!!!");  
                
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));           
                Console.WriteLine("OKOK that's create the response and store it!!!!!!!!!");
            }


        }

        public string GenerateCacheFromRequest(HttpRequest request)
        {
            var keybuilder = new StringBuilder();

            if (request.Path.ToString().Contains("campaign"))
            {
                keybuilder.Append("cacheCampaigns");
                return keybuilder.ToString();
            }


            keybuilder.Append($"{request.Path}");
            

            foreach(var (key,value) in request.Query.OrderBy(x => x.Key))
            {
                keybuilder.Append($"|{key}-{value}");
            }

            return keybuilder.ToString();
        }



    }
}
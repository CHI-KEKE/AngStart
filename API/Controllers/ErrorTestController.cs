using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class ErrorTestController : BaseApiController
    {
        private readonly StoreContext _context;

        public ErrorTestController(StoreContext contexr)
        {
            _context = contexr;
        }

        [HttpGet("NotFound")]
        public ActionResult<Product> GetNotFoundRequest()
        {
            var thing = _context.Products.Find(424242424242);
            if(thing == null)
            {
                return NotFound(new ApiResponse(404));  // happen here
            }

            return Ok();  
        }

        [HttpGet("serverError")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Products.Find(424242424242);
            var thingString = thing.ToString();   //happened here

            return thingString;
        }



        [HttpPost("registerBadRequest")]
        public async Task<ActionResult> Login(RegisterTestDto registertestDto)
        {
            return Ok("RegisterTestDtoAccepted!");
        }


        [HttpGet("badRequest/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            
            return BadRequest();
        }



        [HttpGet("TestingAuth")]
        [Authorize]
        public ActionResult<string> GetSecretStuff()
        {
            return "This is my little secret >__<|||";
        }
    }
}
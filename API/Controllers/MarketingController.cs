using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.Marketing;
using API.Helpers;
using AutoMapper;
using Core.Entities.Marketing;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Memory;
using API.Errors;
using Infrastructure.Repository;

namespace API.Controllers
{

    public class MarketingController : BaseApiController
    {
        private readonly StoreContext _context;
        public IMapper _mapper { get; }
        private readonly IMemoryCache _cache;
        private readonly ILogger<MarketingController> _logger;
        private readonly CampaignRepository _campaignrepo;


        public MarketingController(StoreContext context,IMapper mapper,IMemoryCache cache,ILogger<MarketingController> logger,CampaignRepository campaignrepo)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
            _campaignrepo = campaignrepo;
        }


        // [HttpPost("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromForm] CampaignDto Campaigndto)
        {
            Campaign campaign = _mapper.Map<Campaign>(Campaigndto);

            // Save the campaign to the database
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();

            return Ok(new ApiResponse(201,$"campaign for Product {campaign.Product_Id} has been saved"));
        }


        [HttpGet("campaigns")]
        // [HttpGet]
        public async Task<IActionResult> GetAllCampaigns()
        {
                var Campaigns = _context.Campaigns.ToList();
                return Ok(new ApiResponse(200,$"Successfully get Campaign List",Campaigns));
        }

        // [Cache(600)]

        [HttpGet("campaign")]
        // [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById(long id)
        {

            
            var campaign =  await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == id);
            // _logger.LogInformation("testing campaign log!!!!!!!!!!!!!!!!!");

            if (campaign != null)
            {
                // _cache.Set(cacheKey, campaign, TimeSpan.FromMinutes(30));
                return Ok(new ApiResponse(200,$"Successfully get Campaign {id}",campaign));
            };

            return NotFound();
            
        }

//////////////////////////////
            // string cacheKey = $"product_{id}";

            // if (_cache.TryGetValue(cacheKey, out Campaign cachedProduct))
            // {
            //     // Return data from cache
            //     Console.WriteLine("Found in cache : " + cachedProduct);
            //     Console.WriteLine("Cache table now :" + _cache);
            //     return Ok(cachedProduct);
            // }

////////Update/////////////
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(long productid,string story)
        {
            // Validate updatedProductDto data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the existing product from the database
            var existingCampaign = await _context.Campaigns.FirstOrDefaultAsync(item => item.Product_Id == productid);
            if (existingCampaign == null)
            {
                return NotFound();
            }

            var result = await _campaignrepo.UpdateCampaignStory(existingCampaign,story);
            if(result)
            {
                return Ok(new ApiResponse(204,$"Successfully update Campaign for Product{productid}",existingCampaign));
            }
                return Ok(new ApiResponse(500,$"Update campaign story failed"));

        }

/////////////////////////////
    }
}
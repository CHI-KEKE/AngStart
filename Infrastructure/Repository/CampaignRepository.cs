using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Marketing;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Repository
{
    public class CampaignRepository
    {
         private readonly StoreContext _context;

        public CampaignRepository(StoreContext context)
        {
            _context = context;

        }


          public async Task<bool> UpdateCampaignStory(Campaign campaign, string story)
        {
            
            // Update the product properties with the new data
            campaign.Story = story;
            // ...

            // Save changes to the database
            _context.Campaigns.Update(campaign);
            var result = await _context.SaveChangesAsync();
            return result > 0? true:false;
            
        }      
       
    }
}
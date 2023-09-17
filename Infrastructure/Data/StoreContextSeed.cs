using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Marketing;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if(!context.Products.Any())
            {

                var productsData = File.ReadAllText("../Infrastructure/Data/Seed_Data/store.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);

            }

            if(!context.Products.Where(p => p.Category == "accessories").Any())
            {

                var productsData = File.ReadAllText("../Infrastructure/Data/Seed_Data/acc.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);

            }
    
            if(!context.Campaigns.Any())
            {

                var campaignsData = File.ReadAllText("../Infrastructure/Data/Seed_Data/campaign.json");
                var campaigns = JsonSerializer.Deserialize<List<Campaign>>(campaignsData);
                context.Campaigns.AddRange(campaigns);

            }
						
                        
            if(context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
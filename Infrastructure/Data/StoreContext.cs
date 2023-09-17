using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Marketing;
using Core.Entities.Pay;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext:DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products{get;set;}
        public DbSet<Color> Colors{get;set;}
        public DbSet<Variant> Varients{get;set;}
        public DbSet<Image> Images{get;set;}
        public DbSet<Size> Sizes{get;set;}
        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Order> Orders{get;set;}
        public DbSet<Recipient> Recipients{get;set;}
        public DbSet<Item> Items{get;set;}

    }
}
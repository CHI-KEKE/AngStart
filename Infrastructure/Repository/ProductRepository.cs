using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;

        }

        
        public async Task<Product> GetProductByIdAsync(long id)
        {
            return  await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Colors)
            .Include(p => p.Sizes)
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(pro => pro.Id == id);

        }

        public async Task<ProductData> GetProductsAsync(ProductSpecParams ProductParams)
        {
            var ProductsIQ = _context.Set<Product>().AsQueryable();

             //Filtering

            if(ProductParams.Category != null)
            {
                ProductsIQ = ProductsIQ.Where((pro) => pro.Category == ProductParams.Category).OrderBy((pro) => pro.Price);
            }

            if(ProductParams.keyword != null)
            {
                ProductsIQ = _context.Products.Where(pro => string.IsNullOrEmpty(ProductParams.keyword) ||  pro.Title.Contains(ProductParams.keyword)).OrderBy((pro) => pro.Price);
            }           


            var CountAfterFiltering = await ProductsIQ.CountAsync();  // count
            decimal TotalPageNumber = (decimal)CountAfterFiltering/6;     //count

            var RealTotalPages = (int)Math.Ceiling(TotalPageNumber);

            var CurrentPageProducts = ProductParams.paging == 0 ? 
            ProductsIQ.Take(ProductParams.PageSize).ToList():
            ProductsIQ.Skip(ProductParams.PageSize*ProductParams.paging).Take(ProductParams.PageSize)
            .Include(p => p.Images)
            .Include(p => p.Colors)
            .Include(p => p.Sizes)
            .Include(p => p.Variants).ToList();   


            var productData = new ProductData
            {
                CountAfterFiltering = CountAfterFiltering,
                CurrentPageProducts = CurrentPageProducts,
                TotalPageNumber = TotalPageNumber,
            };

            return productData;
        }
    }


}
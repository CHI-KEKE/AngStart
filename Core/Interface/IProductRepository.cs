using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specification;
namespace Core.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(long id);
        Task<ProductData> GetProductsAsync(ProductSpecParams ProductParams);
    }
}
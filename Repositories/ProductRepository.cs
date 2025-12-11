using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApiDBContext _apiDbContext;
        public ProductRepository(ApiDBContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _apiDbContext.Products.Include(product=>product.Category).ToListAsync();
        }

    }
}

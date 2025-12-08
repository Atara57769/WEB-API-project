using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApiDBContext _apiDbContext;
        public CategoryRepository(ApiDBContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _apiDbContext.Categories.ToListAsync();
        }
    }
}

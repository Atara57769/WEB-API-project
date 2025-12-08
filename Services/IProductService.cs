using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
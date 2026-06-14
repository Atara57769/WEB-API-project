using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Services
{
    public class ProductService : IProductService
    {
        private const string _productsVersionKey = "products_version";
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductService> _logger;


        public ProductService(
            IProductRepository productRepository, 
            IMapper mapper,
            IDistributedCache cache,
            IConfiguration configuration,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<PageResponseDTO<ProductDTO>> GetProducts(int position, int skip, int?[] categoryIds,
            string? description, int? maxPrice, int? minPrice)
        {
            int version = await GetCacheVersion();

            string categoryIdsStr = categoryIds != null
                ? string.Join(",", categoryIds.Where(c => c.HasValue).Select(c => c.Value))
                : "";

            string cacheKey =
                $"products_v{version}_{categoryIdsStr}_{description ?? ""}_{maxPrice ?? 0}_{minPrice ?? 0}_{position}_{skip}";

            var cached = await TryGetFromCache<PageResponseDTO<ProductDTO>>(cacheKey);
            if (cached != null) return cached;

            var (items, totalItems) = await _productRepository.GetProducts(position, skip, categoryIds, description, maxPrice, minPrice);
            List<ProductDTO> data = _mapper.Map<List<Product>, List<ProductDTO>>(items);
            int numOfPages = totalItems / skip;
            if (totalItems % skip != 0)
                numOfPages++;
            PageResponseDTO<ProductDTO> pageResponse = new(
             data,
            totalItems,
            position,
            skip,
            position > 1,
            position < numOfPages
            );

            await TrySetCache(cacheKey, pageResponse);

            return pageResponse;
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            return _mapper.Map<Product, ProductDTO>(product);
        }

        public async Task<ProductDTO> AddProduct(PostProductDTO product)
        {
            var returnedProduct = await _productRepository.AddProduct(_mapper.Map<PostProductDTO, Product>(product));
            var productDto = _mapper.Map<Product, ProductDTO>(returnedProduct);

            if (productDto != null)
            {
                await InvalidateProductCache();
            }

            return productDto;
        }

        private async Task<T?> TryGetFromCache<T>(string key) where T : class
        {
            try
            {
                var json = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache GET failed for key {Key}", key);
            }
            return null;
        }

        private async Task TrySetCache<T>(string key, T value)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                int ttl = _configuration.GetValue<int>("CacheSettings:ProductCacheTTLMinutes");

                await _cache.SetStringAsync(key, json,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ttl)
                    });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache SET failed for key {Key}", key);
            }
        }

        private async Task<int> GetCacheVersion()
        {
            try
            {
                var versionStr = await _cache.GetStringAsync(_productsVersionKey);
                return string.IsNullOrEmpty(versionStr) ? 1 : int.Parse(versionStr);
            }
            catch
            {
                return 1;
            }
        }

        private async Task InvalidateProductCache()
        {
            try
            {
                var versionStr = await _cache.GetStringAsync(_productsVersionKey);
                int version = string.IsNullOrEmpty(versionStr) ? 1 : int.Parse(versionStr);

                await _cache.SetStringAsync(_productsVersionKey, (version + 1).ToString());

                _logger.LogInformation("Product cache invalidated via versioning");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache invalidation failed");
            }
        }

    }
}

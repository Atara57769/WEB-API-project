using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ProductService : IProductService
    {
        private const string _productsVersionKey = "products_version";
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository, 
            IMapper mapper,
            IRedisService redisService,
            IConfiguration configuration,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _redisService = redisService;
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

            var cached = await _redisService.GetAsync<PageResponseDTO<ProductDTO>>(cacheKey);
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

            int ttl = _configuration.GetValue<int>("CacheSettings:ProductCacheTTLMinutes");
            await _redisService.SetAsync(cacheKey, pageResponse, TimeSpan.FromMinutes(ttl));

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

        private async Task<int> GetCacheVersion()
        {
            var versionStr = await _redisService.GetStringAsync(_productsVersionKey);
            return string.IsNullOrEmpty(versionStr) ? 1 : int.Parse(versionStr);
        }

        private async Task InvalidateProductCache()
        {
            var versionStr = await _redisService.GetStringAsync(_productsVersionKey);
            int version = string.IsNullOrEmpty(versionStr) ? 1 : int.Parse(versionStr);

            await _redisService.SetStringAsync(_productsVersionKey, (version + 1).ToString());

            _logger.LogInformation("Product cache invalidated via versioning");
        }
    }
}


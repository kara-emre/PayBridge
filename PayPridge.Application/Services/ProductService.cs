using AutoMapper;
using PayPridge.Application.DTOs;
using PayPridge.Application.Interfaces;
using PayPridge.Domain.Entities;
using PayPridge.Domain.Interfaces;

namespace PayPridge.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;


        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            var result = await _productRepository.AddOrUpdateAsync(product);

            return _mapper.Map<ProductDto>(result);
        }

        public async Task<ProductDto?> FindAsync(string id)
        {
            var result = await _productRepository.GetByIdAsync(id);

            if (result == null)
            {
                return null;
            }

            return _mapper.Map<ProductDto>(result);
        }


        public async Task<ProductResponse> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            var response = _mapper.Map<ProductResponse>(products);

            return response;
        }
    }
}

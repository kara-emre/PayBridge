using PayPridge.Application.DTOs;

namespace PayPridge.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> GetAllProductsAsync();

        Task<ProductDto> AddAsync(ProductDto productDto);

        Task<ProductDto?> FindAsync(string id);

    }
}

using PayPridge.Domain.Entities;

namespace PayPridge.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();  // Mevcut ürünleri al
        Task<Product?> GetByIdAsync(string id);  // Ürün ID'sine göre tek bir ürün al
        Task<Product> AddAsync(Product product);  // Yeni ürün ekle
        Task<Product> UpdateAsync(Product product);  // Ürün güncelle
        Task<Product> AddOrUpdateAsync(Product product);  // Yeni ürün ekle veya mevcut ürünü güncelle
    }
}

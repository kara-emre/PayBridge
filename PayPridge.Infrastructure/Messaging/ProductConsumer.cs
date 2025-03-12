using MassTransit;
using PayPridge.Application.DTOs;
using PayPridge.Application.Interfaces;

namespace PayPridge.Infrastructure.Messaging
{
    public class ProductConsumer : IConsumer<ProductDto>
    {
        private readonly IProductService _productService;

        public ProductConsumer(IProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<ProductDto> context)
        {
            try
            {
                Console.WriteLine($"✅ Product processed: {context.Message.Name}");

                await _productService.AddAsync(context.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error processing product: {ex.Message}");
                throw; // Hata olursa retry mekanizması çalışır
            }
        }
    }
}

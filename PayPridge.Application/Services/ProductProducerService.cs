using MassTransit;
using PayPridge.Application.DTOs;
using PayPridge.Application.Interfaces;
using RestSharp;

namespace PayPridge.Application.Services
{
    public class ProductProducerService : IProductProducerService
    {
        private readonly RestClient _restClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductProducerService(IPublishEndpoint publishEndpoint)
        {
            _restClient = new RestClient();
            _publishEndpoint = publishEndpoint;
        }

        public async Task FetchAndPublishProducts()
        {
            try
            {
                // RestRequest ile GET isteği oluşturuluyor
                var request = new RestRequest("https://balance-management-pi44.onrender.com/api/products", Method.Get);
                var response = await _restClient.ExecuteAsync<ProductApiResponse>(request);

                if (response.IsSuccessful)
                {

                    if (response.Data is not null && response.Data.Products is { Length: > 0 })
                    {
                        foreach (var product in response.Data.Products)
                        {
                            // RabbitMQ'ya mesaj yayınlanıyor
                            await _publishEndpoint.Publish(product);
                        }

                        Console.WriteLine($"{response.Data.Products} products published to RabbitMQ.");
                    }
                    else
                    {
                        Console.WriteLine("No products found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to fetch products: {response.StatusCode} - {response.Content}");
                    throw new Exception($"Failed to fetch products: {response.StatusCode} - {response.Content}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

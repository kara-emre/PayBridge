using System.Text.Json.Serialization;

namespace PayPridge.Application.DTOs
{
    public class ProductApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("data")]
        public ProductDto[] Products { get; set; }
    }
}
namespace PayPridge.Application.DTOs
{
    public record OrderItemDto(string ProductId, decimal Price, int Quantity);
}

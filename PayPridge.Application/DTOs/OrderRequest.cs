namespace PayPridge.Application.DTOs
{
    public record OrderRequest(List<OrderItemDto> Products, decimal TotalAmount);
}

using PayPridge.Application.DTOs;

namespace PayPridge.Domain.Events
{
    public record CreatePreOrderPaymentCommand(Guid OrderId, List<OrderItemDto> OrderItems, decimal TotalPrice);
} 

using PayPridge.Application.DTOs;
using PayPridge.Domain.Events;

namespace PayPridge.Application.Interfaces
{
    public interface IOrderPaymentService
    {
        Task<OrderPaymentConfirmation> CreateOrderAsync(OrderRequest orderRequest);

        Task<OrderPaymentConfirmation> CompleteOrderAsync(Guid id);

    }
}

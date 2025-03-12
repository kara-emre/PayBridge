namespace PayPridge.Domain.Events
{
    public record CompleteOrderPaymentCommand(Guid OrderId);
}

namespace PayPridge.Domain.Events
{
    public record OrderPaymentConfirmation(Guid OrderId, bool IsSuccess, string Message);
}

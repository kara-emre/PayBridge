using MassTransit;
using PayPridge.Domain.Domain;
using PayPridge.Domain.Events;
using Polly;
using Polly.Retry;
using RestSharp;

public class PreOrderConsumer : IConsumer<CreatePreOrderPaymentCommand>
{
    private readonly RestClient _restClient;
    private readonly AsyncRetryPolicy<PreOrderResponse> _retryPolicy;
    public PreOrderConsumer(RestClient restClient)
    {
        _restClient = restClient;
        _retryPolicy = Policy
            .HandleResult<PreOrderResponse>(response => !response.Success)
            .Or<Exception>() // Bağlantı hataları vs.
            .WaitAndRetryAsync(
                retryCount: 5, 
                retryAttempt => TimeSpan.FromMilliseconds(1000 * Math.Pow(2, retryAttempt)),
                (outcome, waitTime, retryNumber, context) =>
                {
                    Console.WriteLine($"🔄 Retry {retryNumber}: Waiting {waitTime.TotalMilliseconds}ms due to {outcome.Exception?.Message ?? outcome.Result.Error}");
                });
    }

    public async Task Consume(ConsumeContext<CreatePreOrderPaymentCommand> context)
    {
        var orderId = context.Message.OrderId;
        Console.WriteLine($"🔄 Processing order {orderId}");

        var request = new RestRequest("https://balance-management-pi44.onrender.com/api/balance/preorder", Method.Post);
        request.AddJsonBody(new { OrderId = orderId, Amount = context.Message.TotalPrice });
        request.Timeout = TimeSpan.FromMinutes(1);

        try
        {
            var response = await _retryPolicy.ExecuteAsync(async () =>
            {
                var restResponse = await _restClient.ExecuteAsync<PreOrderResponse>(request);
                if (restResponse.IsSuccessful && restResponse.Data != null)
                    return restResponse.Data;
                else
                    throw new Exception(restResponse?.Data?.Message);
            });


            if (response.Success)
            {
                await context.RespondAsync(new OrderPaymentConfirmation(orderId, true, response.Message ?? "Pre Order Created"));
                Console.WriteLine("✅ Payment processed successfully!");
                return;
            }
            else
            {
                throw new Exception("❌ Payment Failed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Order processing failed after retries: {ex.Message}");
            await context.RespondAsync(new OrderPaymentConfirmation(Guid.Empty, false, $"Error processing order: {ex.Message}"));
        }

    }
}


using MassTransit;
using PayPridge.Domain.Domain;
using PayPridge.Domain.Events;
using Polly;
using Polly.Retry;
using RestSharp;

namespace PayPridge.Infrastructure.Messaging
{
    public class CompleteOrderConsumer : IConsumer<CompleteOrderPaymentCommand>
    {
        private readonly RestClient _restClient;
        private readonly AsyncRetryPolicy<PreOrderResponse> _retryPolicy;

        public CompleteOrderConsumer(RestClient restClient)
        {
            _restClient = restClient;

            _retryPolicy = Policy
                 .HandleResult<PreOrderResponse>(response => !response.Success)
                 .Or<Exception>() 
                 .WaitAndRetryAsync(
                     retryCount: 5, 
                     retryAttempt => TimeSpan.FromMilliseconds(1000 * Math.Pow(2, retryAttempt)),
                     (outcome, waitTime, retryNumber, context) =>
                     {
                         Console.WriteLine($"🔄 Retry {retryNumber}: Waiting {waitTime.TotalMilliseconds}ms due to {outcome.Exception?.Message ?? outcome.Result.Error}");
                     });
        }

        public async Task Consume(ConsumeContext<CompleteOrderPaymentCommand> context)
        {
            var orderId = context.Message.OrderId;
            Console.WriteLine($"🔄 Processing order {orderId}");

            var request = new RestRequest("https://balance-management-pi44.onrender.com/api/balance/complete", Method.Post);
            request.AddJsonBody(new { OrderId = orderId });
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
                    Console.WriteLine("✅ Order created successfully!");
                    return;
                }
                else
                {
                    throw new Exception("❌ Order Created Failed");
                }
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new OrderPaymentConfirmation(orderId, false, $"Error processing order: {ex.Message}"));
                Console.WriteLine($"❌ Error processing order after retries: {ex.Message}");
            }
        }
    }
}

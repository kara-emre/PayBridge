using Microsoft.Extensions.Hosting;
using PayPridge.Application.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace PayPridge.Infrastructure.BackgroundServices
{
    public class ProductUpdateWorker : BackgroundService
    {
        private readonly IProductProducerService _productProducerService;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10); // Her 10 dakikada bir çalışacak

        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public ProductUpdateWorker(IProductProducerService productProducerService)
        {
            _productProducerService = productProducerService;

            // Polly Retry Policy: 3 kez retry yap, her denemede bekleme süresini artır (2, 4, 8 saniye)
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"⚠️ Retry {retryCount} after {timeSpan.TotalSeconds}s due to {exception.Message}");
                    });

            // Polly Circuit Breaker Policy: 3 hata olursa 30 saniyeliğine işlemi durdur
            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                    onBreak: (exception, timespan) =>
                    {
                        Console.WriteLine($"⛔ Circuit Breaker OPEN - Pausing for {timespan.TotalSeconds}s due to {exception.Message}");
                    },
                    onReset: () => Console.WriteLine("✅ Circuit Breaker RESET - Resuming operations"),
                    onHalfOpen: () => Console.WriteLine("⚠️ Circuit Breaker HALF-OPEN - Testing system"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(_interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("🔄 Fetching & Publishing Products...");

                    // Polly ile retry ve circuit breaker uygula
                    await _retryPolicy
                        .WrapAsync(_circuitBreakerPolicy)
                        .ExecuteAsync(() => _productProducerService.FetchAndPublishProducts());

                    Console.WriteLine("✅ Product update successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error in background worker: {ex.Message}");
                }

                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }
    }
}

using System;
using System.Text.Json.Serialization;

namespace PayPridge.Domain.Domain
{
    public class CompleteOrderResponse : BalanceApiBaseResponse
    {
        [JsonPropertyName("data")]
        public Data CompleteOrder { get; set; }

        public class Data
        {
            [JsonPropertyName("order")]
            public Order Order { get; set; }

            [JsonPropertyName("updatedBalance")]
            public UpdatedBalance UpdatedBalance { get; set; }
        }

        public class Order
        {
            [JsonPropertyName("orderId")]
            public Guid OrderId { get; set; }

            [JsonPropertyName("amount")]
            public int Amount { get; set; }

            [JsonPropertyName("timestamp")]
            public DateTime Timestamp { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }

            [JsonPropertyName("completedAt")]
            public DateTime CompletedAt { get; set; }

            [JsonPropertyName("cancelledAt")]
            public DateTime CancelledAt { get; set; }
        }

        public class UpdatedBalance
        {
            [JsonPropertyName("userId")]
            public Guid UserId { get; set; }

            [JsonPropertyName("totalBalance")]
            public long TotalBalance { get; set; }

            [JsonPropertyName("availableBalance")]
            public long AvailableBalance { get; set; }

            [JsonPropertyName("blockedBalance")]
            public long BlockedBalance { get; set; }

            [JsonPropertyName("currency")]
            public string Currency { get; set; }

            [JsonPropertyName("lastUpdated")]
            public DateTime LastUpdated { get; set; }
        }
    }
}

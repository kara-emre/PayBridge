using System.Text.Json.Serialization;

namespace PayPridge.Domain.Domain
{
    public class PreOrderResponse : BalanceApiBaseResponse
    {
        [JsonPropertyName("data")]
        public Data OrderData { get; set; }

        public class Data
        {
            [JsonPropertyName("preOrder")]
            public PreOrder PreOrder { get; set; }

            [JsonPropertyName("updatedBalance")]
            public UpdatedBalance UpdatedBalance { get; set; }
        }

        public class PreOrder
        {
            [JsonPropertyName("orderId")]
            public Guid OrderId { get; set; }

            [JsonPropertyName("amount")]
            public decimal Amount { get; set; }

            [JsonPropertyName("timestamp")]
            public DateTime Timestamp { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }
        }

        public class UpdatedBalance
        {
            [JsonPropertyName("userId")]
            public Guid UserId { get; set; }

            [JsonPropertyName("totalBalance")]
            public decimal TotalBalance { get; set; }

            [JsonPropertyName("availableBalance")]
            public decimal AvailableBalance { get; set; }

            [JsonPropertyName("blockedBalance")]
            public decimal BlockedBalance { get; set; }

            [JsonPropertyName("currency")]
            public string Currency { get; set; }

            [JsonPropertyName("lastUpdated")]
            public DateTime LastUpdated { get; set; }
        }
    }
}

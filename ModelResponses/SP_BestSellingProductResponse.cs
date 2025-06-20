using Newtonsoft.Json;

namespace WebAPISalesManagement.ModelResponses
{
    public class SP_BestSellingProductResponse
    {
        [JsonProperty("product_id")]
        public Guid ProductId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; } = string.Empty;

        [JsonProperty("total_quantity_sold")]
        public int TotalQuantitySold { get; set; }

        [JsonProperty("total_revenue")]
        public long TotalRevenue { get; set; }
    }

}

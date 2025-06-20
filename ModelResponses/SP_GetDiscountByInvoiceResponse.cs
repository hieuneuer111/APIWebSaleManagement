using Newtonsoft.Json;

namespace WebAPISalesManagement.ModelResponses
{
    public class SP_GetDiscountByInvoiceResponse
    {
        [JsonProperty("discounts_id")]
        public Guid DiscountsId { get; set; }
        [JsonProperty("discounts_name")]
        public string? DiscountsName { get; set; }
        [JsonProperty("discounts_percentage")]
        public int? DiscountsPercentage { get; set; }
        [JsonProperty("discounts_fixed_amount")]
        public int? DiscountsFixedAmount { get; set; }
        [JsonProperty("discounts_description")]
        public string? DiscountsDescription { get; set; }
        [JsonProperty("discount_isactive")]
        public bool? DiscountsIsActive { get; set; }
        
    }
}

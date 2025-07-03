using Newtonsoft.Json;

namespace WebAPISalesManagement.ModelResponses
{
    public class ToppingDetail
    {
        [JsonProperty("topping_id")]
        public Guid ToppingId { get; set; }

        [JsonProperty("topping_name")]
        public string? ToppingName { get; set; }

        [JsonProperty("topping_price")]
        public int ToppingPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class SP_GetProductByInvoiceResponce
    {
        [JsonProperty("sp_pro_id")]
        public Guid ProductId { get; set; }

        [JsonProperty("sp_pro_name")]
        public string? ProductName { get; set; }

        [JsonProperty("sp_dis_id")]
        public Guid? DiscountId { get; set; }

        [JsonProperty("sp_dis_name")]
        public string? DiscountName { get; set; }

        [JsonProperty("sp_percentage")]
        public long? Percentage { get; set; }

        [JsonProperty("sp_fixed_amount")]
        public long? FixedAmount { get; set; }

        [JsonProperty("sp_dis_description")]
        public string? DiscountDescription { get; set; }

        [JsonProperty("sp_line_total")]
        public long? LineTotal { get; set; }

        [JsonProperty("sp_unit_price")]
        public long? UnitPrice { get; set; }

        [JsonProperty("sp_quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("sp_total_price")]
        public long? TotalPrice { get; set; }

        [JsonProperty("sp_toppings")]
        public List<ToppingDetail>? Toppings { get; set; }
    }
}


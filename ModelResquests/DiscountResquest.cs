namespace WebAPISalesManagement.ModelResquests
{
    public class DiscountResquest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Percentage { get; set; }
        public int? FixedAmount { get; set; }
        public int? MinInvoiceTotal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

namespace WebAPISalesManagement.ModelResponses
{
    public class DiscountResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Percentage { get; set; }
        public long? FixedAmount { get; set; }
        public long? MinInvoiceTotal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}

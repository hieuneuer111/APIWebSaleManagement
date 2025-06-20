namespace WebAPISalesManagement.ModelResponses
{
    public class InvoiceListResponse
    {
        public Guid InvoiceId { get; set; }
        public bool InvoiceStatus { get; set; }
        public long TotalAmount { get; set; }
        public long FinalTotal { get; set; }
        public UserResponse InvoiceCreater { get; set; }
        public DateTime CreatedAt { get; set; }
        public long DiscountValue { get; set; }
        public Guid? Discount_Id { get; set; }
        public string? DiscountSnapshotName { get; set; }
        public string? DiscountType { get; set; }
        public string? DiscountRawValue { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? PaymentTime { get; set; }

    }
}

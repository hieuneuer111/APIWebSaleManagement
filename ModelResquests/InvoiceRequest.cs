namespace WebAPISalesManagement.ModelResquests
{
    public class InvoiceRequest
    {
        public Guid InvoiceCreater { get; set; }
        public bool Status { get; set; }
        public Guid? DiscountId { get; set; } // Mã giảm cho cả hóa đơn
        public List<InvoiceItemRequest> Items { get; set; } = new();
    }

    public class InvoiceItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid? DiscountId { get; set; } // có thể null
    }
}

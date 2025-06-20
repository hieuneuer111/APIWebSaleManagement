namespace WebAPISalesManagement.ModelResquests
{
    public class FindInvoiceResquest
    {
        public Guid InvoiceCreater {  get; set; }
        public bool Status { get; set; }
        public long TotalAmount { get; set; }
        public long TotalDiscount { get; set; }
        public long total {  get; set; }
        public DateTime CreateAt { get; set; }
    }
}

namespace WebAPISalesManagement.ModelResquests
{
    public class InvoicePaymentRequest
    {
        public Guid InvoiceId { get; set; }
        public string PaymentMethod { get; set; } = "cash"; // "cash" | "transfer"
    }
}

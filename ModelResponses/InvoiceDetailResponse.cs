using Supabase.Postgrest.Models;

namespace WebAPISalesManagement.ModelResponses
{
    public class InvoiceDetailResponse
    {
        public Guid Invoice_Id { get; set; }   
        public bool InvoiceStatus { get; set; }
        public long TotalAmount { get; set; }
        public long FinalTotal { get; set; }
        public Guid? Discount_Id { get; set; }
        public string? DiscountSnapshotName { get; set; }
        public string? DiscountType { get; set; }
        public string? DiscountRawValue { get; set; }
        public long DiscountValue { get; set; }
        public DateTime InvoiceTimeCreate { get; set; }
        public UserResponse UserCreater { get; set; }
        public List<SP_GetProductByInvoiceResponce>? ProductInvoice {  get; set; }
        public List<SP_GetDiscountByInvoiceResponse>? DiscountInvoice {  get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? PaymentTime { get; set; }
    }
}

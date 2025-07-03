namespace WebAPISalesManagement.ModelResquests
{
    public class InvoiceRequest
    {
        public Guid InvoiceCreater { get; set; }
        public bool Status { get; set; }
        public Guid? DiscountId { get; set; } // Mã giảm giá toàn hóa đơn
        public List<InvoiceItemRequest> Items { get; set; } = new();
    }

    public class InvoiceItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid? DiscountId { get; set; } // Giảm giá từng sản phẩm (nếu có)
        public List<InvoiceToppingRequest> Toppings { get; set; } = new(); // Danh sách topping đi kèm
    }

    public class InvoiceToppingRequest
    {
        public Guid ToppingId { get; set; }
        public int Quantity { get; set; }
    }
}

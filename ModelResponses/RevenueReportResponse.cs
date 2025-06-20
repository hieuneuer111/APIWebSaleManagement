namespace WebAPISalesManagement.ModelResponses
{
    public class RevenueReportResponse
    {
        public int TotalOrders { get; set; }
        public long TotalAmount { get; set; }
        public long TotalDiscount { get; set; }
        public long TotalRevenue { get; set; }
    }
}

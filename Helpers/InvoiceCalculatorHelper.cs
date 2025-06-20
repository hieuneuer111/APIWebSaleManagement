using WebAPISalesManagement.Models;

namespace WebAPISalesManagement.Helpers
{
    public static class DiscountHelper
    {
        public static async Task<(bool isValid, string? errorMessage)> ValidateDiscountAsync(
            DiscountsModel discount, long currentTotalAmount)
        {
            if (discount == null)
                return (false, "Mã giảm giá không tồn tại.");

            if (discount.IsActive != true)
                return (false, $"Mã giảm giá '{discount.Name}' đã bị hủy.");

            if (discount.StartDate.HasValue && DateTime.Now < discount.StartDate.Value)
                return (false, $"Mã giảm giá '{discount.Name}' chưa có hiệu lực.");

            if (discount.EndDate.HasValue && DateTime.Now > discount.EndDate.Value)
                return (false, $"Mã giảm giá '{discount.Name}' đã hết hạn.");

            if (discount.MinInvoiceTotal.HasValue && currentTotalAmount < discount.MinInvoiceTotal.Value)
                return (false, $"Mã giảm giá '{discount.Name}' chỉ áp dụng cho đơn hàng từ {discount.MinInvoiceTotal.Value:N0}đ.");

            return (true, null);
        }
    }
}

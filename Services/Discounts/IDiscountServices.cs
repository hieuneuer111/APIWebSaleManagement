using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Discounts
{
    public interface IDiscountServices
    {
        Task<ModelDataPageResponse<List<DiscountResponse>>> GetDiscountListAsync(string search,int typeDiscount, int PageNumber, int PageSize, bool isPaging, DateTime? dateStart, DateTime? dateEnd, int valid);
        Task<DiscountResponse> GetDiscountItemsAsync(Guid discountId);
        Task<ModelResponse> ExtendDiscountAsync(Guid discountId, DateTime newDateEnd);
        Task<ModelResponse> ActiveDiscountAsync(Guid discountId, bool newStatus);
    }
}

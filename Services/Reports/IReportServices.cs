using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Reports
{
    public interface IReportServices
    {
        Task<ModelDataResponse<RevenueReportResponse>> GetRevenueReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<ModelDataResponse<List<SP_BestSellingProductResponse>>> BestSellingProduct(DateTime? dateFrom, DateTime? dateEnd, int top);
    }
}

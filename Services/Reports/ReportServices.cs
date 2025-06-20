using Supabase.Postgrest.Responses;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.Authorization;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Reports
{
    public class ReportServices: IReportServices
    {
        private readonly ISupabaseClientService _supabaseClientService;
        private readonly IAuthService _authService;
        private readonly Supabase.Client _clientSupabase;
        public ReportServices(Supabase.Client clientSupabase, ISupabaseClientService supabaseClientService, IAuthService authService)
        {
            _clientSupabase = clientSupabase;
            _supabaseClientService = supabaseClientService;
            _authService = authService;
        }
        public async Task<ModelDataResponse<List<SP_BestSellingProductResponse>>> BestSellingProduct(DateTime? dateFrom, DateTime? dateEnd,int top)
        {
            ModelDataResponse<List<SP_BestSellingProductResponse>> result = new ModelDataResponse<List<SP_BestSellingProductResponse>>();
            List<SP_BestSellingProductResponse> listProductTop = await _supabaseClientService.GetBestSellingProduct(dateFrom, dateEnd,top);
            if(listProductTop != null)
            {
                result.IsValid = true;
                result.ValidationMessages.Add("Success");
                result.ItemResponse = listProductTop;
            }
            return result;
        }
        public async Task<ModelDataResponse<RevenueReportResponse>> GetRevenueReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            ModelDataResponse<RevenueReportResponse> response = new ModelDataResponse<RevenueReportResponse>();
            try
            {
                
                var query = _clientSupabase
                    .From<InvoicesModel>()
                    .Select("*");

                if (fromDate.HasValue)
                    query = query.Filter("created_at", Supabase.Postgrest.Constants.Operator.GreaterThanOrEqual, fromDate.Value.ToString("yyyy-MM-dd"));

                if (toDate.HasValue)
                    query = query.Filter("created_at", Supabase.Postgrest.Constants.Operator.LessThanOrEqual, toDate.Value.ToString("yyyy-MM-dd"));

                var invoices = await query.Get();

                var totalOrders = invoices.Models.Count;
                var totalAmount = invoices.Models.Sum(i => i.Total_Amount);
                var totalDiscount = invoices.Models.Sum(i => i.Discount_Value);
                var totalRevenue = invoices.Models.Sum(i => i.Final_Total);
                RevenueReportResponse responseList = new RevenueReportResponse
                {
                    TotalOrders = totalOrders,
                    TotalAmount = totalAmount,
                    TotalDiscount = totalDiscount,
                    TotalRevenue = totalRevenue
                };
                response.IsValid = true;
                response.ValidationMessages.Add("Success");
                response.ItemResponse = responseList;
            }
            catch (Exception ex) {
                response.IsValid = false;
                response.ValidationMessages.Add(ex.Message);
            }
            return response;
        }

    }
}

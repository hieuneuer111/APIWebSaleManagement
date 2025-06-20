using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Discounts
{
    public class DiscountServices:IDiscountServices
    {
        private readonly Supabase.Client _supabaseClient;
        public DiscountServices(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<ModelDataPageResponse<List<DiscountResponse>>> GetDiscountListAsync(string search, int typeDiscount, int PageNumber, int PageSize, bool isPaging,DateTime? dateStart, DateTime? dateEnd, int valid)
        {
            ModelDataPageResponse<List<DiscountResponse>> result = new ModelDataPageResponse<List<DiscountResponse>>();
            try
            {
                ModeledResponse<DiscountsModel> SupabaseResponse = await _supabaseClient.From<DiscountsModel>().Get();
                List<DiscountsModel> ListItems = SupabaseResponse.Models.ToList();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    StringConvert conv = new StringConvert();
                    search = conv.ConvertToUnSign(search);
                    ListItems = ListItems.Where(tb => conv.ConvertToUnSign(tb.Name).Contains(search)).ToList();
                }
                // Nếu = 1 thì lọc tất cả Giảm giá theo % "Percentage", = 2 thì theo giá tiền "FixedAmount"
                if (typeDiscount == 1)
                {
                    ListItems = ListItems.Where(tb => tb.FixedAmount == null || tb.FixedAmount == 0).ToList();
                }
                else if (typeDiscount == 2) {
                    ListItems = ListItems.Where(tb => tb.Percentage == null || tb.Percentage == 0).ToList();
                }
                // Lọc theo ngày
                if (dateStart.HasValue && dateEnd.HasValue)
                {
                    ListItems = ListItems.Where(i => i.StartDate >= dateStart && i.EndDate <= dateEnd).ToList();
                }
                else if (dateStart.HasValue)
                {
                    ListItems = ListItems.Where(i => i.StartDate >= dateStart).ToList();
                }
                else if (dateEnd.HasValue)
                {
                    ListItems = ListItems.Where(i => i.EndDate <= dateEnd).ToList();
                }
                // Nếu cả hai đều null thì không filter theo ngày
                // Lọc theo còn hạn hay không valid == 1 còn hạn, valid == 2 hết hạn  
                DateTime dateNow = DateTime.Now;
                if (valid == 1)
                {
                    ListItems = ListItems.Where(i => i.StartDate <= dateNow && i.EndDate >= dateNow).ToList();
                }
                else if (valid == 2) {
                    ListItems = ListItems.Where(i => i.StartDate > dateNow || i.EndDate < dateNow).ToList();
                }
                List<DiscountResponse> resultsItemResponse = ListItems.Select(u => new DiscountResponse
                {
                      Name = u.Name,
                      CreatedAt = u.CreatedAt,
                      Description = u.Description,
                      EndDate = u.EndDate,
                      FixedAmount = u.FixedAmount,
                      Id = u.Id,
                      MinInvoiceTotal = u.MinInvoiceTotal,
                      Percentage = u.Percentage,
                      StartDate = u.StartDate,
                      IsActive = u.IsActive,
                }).ToList();
                result =
                    Helpers.PaginationHelper.createPageDataResponse<List<DiscountResponse>>(resultsItemResponse.Count, PageNumber, PageSize, false);
                resultsItemResponse = isPaging ? resultsItemResponse.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : resultsItemResponse;
                result.items = resultsItemResponse;
               
            }
            catch (Exception ex) {           
            }
            return result;
        }
        public async Task<DiscountResponse> GetDiscountItemsAsync(Guid discountId)
        {
            DiscountResponse discountResult = new DiscountResponse();
            // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
            ModeledResponse<DiscountsModel> supabaseResponse = await _supabaseClient
                .From<DiscountsModel>()
                .Where(c => c.Id == discountId)  // Sử dụng điều kiện để lọc theo categoryID
                .Get();
            // Kiểm tra xem có dữ liệu không
            if (supabaseResponse.Models != null && supabaseResponse.Models.Any())
            {
                // Chuyển kết quả từ SupabaseResponse thành List và gán cho response
                DiscountsModel itemsResponse = supabaseResponse.Models.FirstOrDefault();
                DiscountResponse resultsItemResponse = new DiscountResponse
                {
                   Id = itemsResponse.Id,
                   FixedAmount = itemsResponse.FixedAmount,
                   Percentage = itemsResponse.Percentage,
                   CreatedAt = itemsResponse.CreatedAt,
                   Description = itemsResponse.Description,
                   EndDate = itemsResponse.EndDate,
                   MinInvoiceTotal = itemsResponse.MinInvoiceTotal,
                   Name = itemsResponse.Name,
                   StartDate = itemsResponse.StartDate ,
                   IsActive = itemsResponse.IsActive,
                };
                // Gán dữ liệu vào categoriesResponse (Giả sử CategoriesResponse có một danh sách CategoriesItems)
                discountResult = resultsItemResponse;
            }
            // Trả về response
            return discountResult;
        }
        //extend
        public async Task<ModelResponse> ExtendDiscountAsync(Guid discountId, DateTime newDateEnd)
        {
            ModelResponse response = new ModelResponse();
            var a = new DateTime(newDateEnd.Year, newDateEnd.Month, newDateEnd.Day);
            var s = Helpers.StringConvert.DateTimeConvert(newDateEnd, true);
            ModeledResponse<DiscountsModel> updateResponse = await _supabaseClient
                              .From<DiscountsModel>()
                              .Where(x => x.Id == discountId)
                              .Set(x => x.EndDate, new DateTime(newDateEnd.Year,newDateEnd.Month,newDateEnd.Day) )
                              .Update();
            if (updateResponse == null || updateResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("extend Errors. Discount is empty");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("extended Success to date!"+ Helpers.StringConvert.DateTimeConvert(newDateEnd,true));
            }
            return response;
        }
        //IsActiveDiscount
        public async Task<ModelResponse> ActiveDiscountAsync(Guid discountId, bool newStatus)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<DiscountsModel> updateResponse = await _supabaseClient
                              .From<DiscountsModel>()
                              .Where(x => x.Id == discountId)
                              .Set(x => x.IsActive, newStatus)
                              .Update();
            if (updateResponse == null || updateResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add(" Errors. Discount is empty");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add(" Success!");
            }
            return response;
        }
    }
}

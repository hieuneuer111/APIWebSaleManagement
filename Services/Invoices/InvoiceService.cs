
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using System.Text.Json;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.Authorization;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Swagger;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAPISalesManagement.Services.Invoices
{
    public class InvoiceService:IInvoiceService
    {
        private readonly ISupabaseClientService _supabaseClientService;
        private readonly IAuthService _authService;
        private readonly Supabase.Client _clientSupabase;
        public InvoiceService(Supabase.Client clientSupabase, ISupabaseClientService supabaseClientService, IAuthService authService)
        {
            _clientSupabase = clientSupabase;
            _supabaseClientService = supabaseClientService;
            _authService = authService;
        }

        public async Task<ModelDataResponse<List<SP_GetProductByInvoiceResponce>>> GetProductByInvoiceId(Guid invoiceId)
        {
            ModelDataResponse<List<SP_GetProductByInvoiceResponce>> modelDataResponse = new ModelDataResponse<List<SP_GetProductByInvoiceResponce>>();
            try
            {
                modelDataResponse.ItemResponse = await _supabaseClientService.GetProductByInvoice(invoiceId);
            }
            catch (Exception ex)
            {
                modelDataResponse.IsValid = false;
                modelDataResponse.ValidationMessages.Add(ex.Message);
            }
            return modelDataResponse;
        }
        public async Task<ModelDataResponse<InvoiceDetailResponse>> GetInvoiceById(Guid invoiceId)
        {
            ModelDataResponse<InvoiceDetailResponse> modelDataResponse = new ModelDataResponse<InvoiceDetailResponse>();
            try
            {
                // Lấy danh sách từ Supabase
                ModeledResponse<InvoicesModel> supabaseResponse = await _clientSupabase.From<InvoicesModel>().Where(i => i.Invoice_Id == invoiceId).Get();
                InvoicesModel? supabaseInvoice = supabaseResponse.Models.FirstOrDefault();
                if(supabaseInvoice != null)
                {
                    List<SP_GetProductByInvoiceResponce> listProductInvoice = await _supabaseClientService.GetProductByInvoice(supabaseInvoice.Invoice_Id);
                    List<SP_GetDiscountByInvoiceResponse> listDiscountInvoice = await _supabaseClientService.GetDiscountByInvoice(supabaseInvoice.Invoice_Id);
                    UserResponse userResponse = await _authService.GetUserInfoById(supabaseInvoice.Invoice_Creater);
                    modelDataResponse.ItemResponse =  new InvoiceDetailResponse
                    {
                       TotalAmount = supabaseInvoice.Total_Amount,
                       DiscountInvoice = listDiscountInvoice,
                       FinalTotal= supabaseInvoice.Final_Total,
                       Invoice_Id= supabaseInvoice.Invoice_Id,
                       InvoiceStatus = supabaseInvoice.Invoice_Status,
                       DiscountValue= supabaseInvoice.Discount_Value,
                       UserCreater= userResponse,
                       InvoiceTimeCreate = supabaseInvoice.Created_at,
                       ProductInvoice= listProductInvoice,
                       Discount_Id= supabaseInvoice.Discount_Id,
                       DiscountType= supabaseInvoice.Discount_Type, 
                       DiscountRawValue = supabaseInvoice.Discount_Raw_Value,
                       DiscountSnapshotName = supabaseInvoice.Discount_Snapshot_Name,
                      PaymentTime = supabaseInvoice.Payment_Time,
                      PaymentMethod =supabaseInvoice.Payment_Method,
                    };
                }
            }
            catch (Exception ex)
            {
                modelDataResponse.IsValid = false;
                modelDataResponse.ValidationMessages.Add(ex.Message);
            }
            return modelDataResponse;
        }
        public async Task<ModelDataPageResponse<List<InvoiceListResponse>>> GetInvoiceList(DateTime? dateStart, DateTime? dateEnd, List<string> userId, int PageNumber, int PageSize, bool isPaging, bool isDescendPrice, int isStatus)
        {
            // Mặc định dateStart và dateEnd null lọc all
            // nếu dateStart tồn tại thì lọc từ dateStart tới Nay
            // Nếu dateEnd tồn tại thì lọc từ trước tới dateEnd
            // Nếu cả 2 tồn tại thì lọc từ dateStart tới dateEnd
            // Lấy danh sách Sản phẩm từ Supabase
            ModeledResponse<InvoicesModel> SupabaseResponseInvoices = await _clientSupabase.From<InvoicesModel>().Get();
            List<InvoicesModel> SupabaseListInvoices = SupabaseResponseInvoices.Models.ToList();
            //Lọc theo trạng thái 1 = đã thanh toán, 2 = chưa thanh toán, 
            if (isStatus == 1)
            {
                SupabaseListInvoices = SupabaseListInvoices.Where(i => i.Invoice_Status == true).ToList();
            }
            else if (isStatus == 2) {
                SupabaseListInvoices = SupabaseListInvoices.Where(i => i.Invoice_Status == false).ToList();
            }
            // Lọc theo ngày
            if (dateStart.HasValue && dateEnd.HasValue)
            {
                SupabaseListInvoices = SupabaseListInvoices.Where(i => i.Created_at >= dateStart && i.Created_at <= dateEnd).ToList();
            }
            else if (dateStart.HasValue)
            {
                SupabaseListInvoices = SupabaseListInvoices.Where(i => i.Created_at >= dateStart).ToList();
            }
            else if (dateEnd.HasValue)
            {
                SupabaseListInvoices = SupabaseListInvoices.Where(i => i.Created_at <= dateEnd).ToList();
            }
            // Nếu cả hai đều null thì không filter theo ngày
            // Lọc theo danh mục (nếu có)
            if (userId != null)
            {
                if (userId.Count() > 0)
                    SupabaseListInvoices = SupabaseListInvoices.Where(tb => userId.Contains(tb.Invoice_Creater.ToString())).ToList();
            }
            // Sử dụng Task.WhenAll để đợi các tác vụ bất đồng bộ
            List<Task<InvoiceListResponse>> ResponsesTasks = SupabaseListInvoices.Select(async (InvoicesModel item) =>
            {
                UserResponse userResponse = await _authService.GetUserInfoById(item.Invoice_Creater);
                return new InvoiceListResponse
                {
                  InvoiceId = item.Invoice_Id,
                  FinalTotal = item.Final_Total,
                  CreatedAt = item.Created_at,
                  DiscountValue = item.Discount_Value,
                  InvoiceCreater = userResponse,
                  InvoiceStatus = item.Invoice_Status,
                  TotalAmount = item.Total_Amount,
                  DiscountRawValue = item.Discount_Raw_Value,
                  DiscountSnapshotName = item.Discount_Snapshot_Name,
                  DiscountType = item.Discount_Type,
                  Discount_Id=item.Discount_Id,
                  PaymentMethod = item.Payment_Method,
                  PaymentTime = item.Payment_Time,
                };
            }).ToList();

            // Đợi tất cả các tác vụ hoàn tất và trả về kết quả
            List<InvoiceListResponse> invoiceList = (await Task.WhenAll(ResponsesTasks)).ToList();
            if (isDescendPrice)
            {
                invoiceList = invoiceList.OrderByDescending(c => c.FinalTotal).ToList();
            }
            else
            {
                invoiceList = invoiceList.OrderBy(c => c.FinalTotal).ToList();
            }
            ModelDataPageResponse<List<InvoiceListResponse>> result =
               Helpers.PaginationHelper.createPageDataResponse<List<InvoiceListResponse>>(invoiceList.Count, PageNumber, PageSize, false);
            invoiceList = isPaging ? invoiceList.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : invoiceList;
            result.items = invoiceList;
            return result;
        }

        public async Task<ModelDataResponse<Guid?>> CreateInvoiceAsync(InvoiceRequest request)
        {
            var result = new ModelDataResponse<Guid?>();
            var invoiceItems = new List<Dictionary<string, object>>();
            long totalAmount = 0;     // Tổng tiền gốc (sản phẩm + topping)
            long discountTotal = 0;   // Tổng giảm giá từng item

            foreach (var item in request.Items)
            {
                // 1. Lấy thông tin sản phẩm
                var product = await _clientSupabase
                    .From<ProductsModel>()
                    .Where(p => p.Product_Id == item.ProductId)
                    .Single();

                if (product == null)
                {
                    result.IsValid = false;
                    result.ValidationMessages.Add($"Không tìm thấy sản phẩm {item.ProductId}");
                    return result;
                }

                long unitPrice = product.Product_Price;
                int quantity = item.Quantity;
                long lineTotal = unitPrice * quantity; // Giá gốc (chưa giảm)

                // 2. Áp dụng giảm giá sản phẩm nếu có
                long discountValue = 0;
                string? discountSnapshotName = null, discountType = null, discountRawValue = null;
                Guid? discountId = null;

                if (item.DiscountId.HasValue)
                {
                    var discount = await _clientSupabase
                        .From<DiscountsModel>()
                        .Where(d => d.Id == item.DiscountId.Value)
                        .Single();

                    var (isValid, message) = await DiscountHelper.ValidateDiscountAsync(discount, lineTotal);
                    if (!isValid)
                    {
                        result.IsValid = false;
                        result.ValidationMessages.Add(message!);
                        return result;
                    }

                    discountId = discount.Id;
                    discountSnapshotName = discount.Name;
                    discountType = discount.Percentage > 0 ? "percentage" : "fixed";
                    discountRawValue = discountType == "percentage"
                        ? $"{discount.Percentage}%"
                        : $"{discount.FixedAmount}đ";

                    discountValue = discountType == "percentage"
                        ? lineTotal * discount.Percentage.Value / 100
                        : discount.FixedAmount ?? 0;

                    discountValue = Math.Min(discountValue, lineTotal);
                }

                // 3. Tính tổng tiền topping
                long toppingTotal = 0;
                var toppingList = new List<Dictionary<string, object>>();

                if (item.Toppings != null && item.Toppings.Any())
                {
                    foreach (var topping in item.Toppings)
                    {
                        var toppingModel = await _clientSupabase
                            .From<ToppingModel>()
                            .Where(t => t.Id == topping.ToppingId)
                            .Single();

                        if (toppingModel == null)
                        {
                            result.IsValid = false;
                            result.ValidationMessages.Add($"Topping {topping.ToppingId} không tồn tại.");
                            return result;
                        }

                        long toppingAmount = toppingModel.Price * topping.Quantity;
                        toppingTotal += toppingAmount;

                        toppingList.Add(new Dictionary<string, object>
                {
                    { "topping_id", toppingModel.Id },
                    { "topping_name", toppingModel.Name },
                    { "topping_price", toppingModel.Price },
                    { "quantity", topping.Quantity },
                    { "total", toppingAmount }
                });
                    }
                }

                // 4. Tổng tiền thực tế cho item = (giá gốc - giảm giá) + topping
                long totalPrice = (lineTotal - discountValue) + toppingTotal;

                // 5. Tạo dữ liệu item hóa đơn
                var invoiceItem = new Dictionary<string, object>
        {
            { "product_id", item.ProductId },
            { "quantity", quantity },
            { "unit_price", (int)unitPrice },
            { "line_total", (int)lineTotal },
            { "discount_id", discountId?.ToString() ?? "" },
            { "discount_snapshot_name", discountSnapshotName },
            { "discount_type", discountType },
            { "discount_raw_value", discountRawValue },
            { "discount_value", (int)discountValue },
            { "total_price", (int)totalPrice }
        };

                if (toppingList.Count > 0)
                {
                    invoiceItem.Add("toppings", toppingList);
                }

                invoiceItems.Add(invoiceItem);
                totalAmount += totalPrice;      // ✅ Cộng vào tổng tiền hóa đơn
                discountTotal += discountValue; // ✅ Cộng vào tổng giảm giá
            }

            // 6. Áp dụng giảm giá toàn hóa đơn (nếu có)
            long invoiceDiscountValue = 0;
            string? invoiceDiscountName = null, invoiceDiscountType = null, invoiceDiscountRawValue = null;
            Guid? invoiceDiscountId = request.DiscountId;

            if (invoiceDiscountId.HasValue)
            {
                var invoiceDiscount = await _clientSupabase
                    .From<DiscountsModel>()
                    .Where(d => d.Id == invoiceDiscountId.Value)
                    .Single();

                var (isValid, message) = await DiscountHelper.ValidateDiscountAsync(invoiceDiscount, totalAmount);
                if (!isValid)
                {
                    result.IsValid = false;
                    result.ValidationMessages.Add(message!);
                    return result;
                }

                invoiceDiscountName = invoiceDiscount.Name;
                invoiceDiscountType = invoiceDiscount.Percentage > 0 ? "percentage" : "fixed";
                invoiceDiscountRawValue = invoiceDiscountType == "percentage"
                    ? $"{invoiceDiscount.Percentage}%"
                    : $"{invoiceDiscount.FixedAmount}đ";

                invoiceDiscountValue = invoiceDiscountType == "percentage"
                    ? totalAmount * invoiceDiscount.Percentage.Value / 100
                    : invoiceDiscount.FixedAmount ?? 0;

                invoiceDiscountValue = Math.Min(invoiceDiscountValue, totalAmount);
            }

            long finalTotal = totalAmount - invoiceDiscountValue;

            // 7. Chuẩn bị dữ liệu gửi vào store procedure
            var invoiceJson = new Dictionary<string, object>
    {
        { "invoice_creater", request.InvoiceCreater.ToString() },
        { "status", request.Status },
        { "total_amount", totalAmount },
        { "discount_value", invoiceDiscountValue },
        { "final_total", finalTotal },
        { "discount_id", invoiceDiscountId?.ToString() ?? "" },
        { "discount_snapshot_name", invoiceDiscountName },
        { "discount_type", invoiceDiscountType },
        { "discount_raw_value", invoiceDiscountRawValue },
        { "items", invoiceItems }
    };

            // 8. Gọi Supabase store để tạo hóa đơn
            Guid? guidNewInvoice = await _supabaseClientService.CreateInvoice(invoiceJson);

            if (guidNewInvoice != Guid.Empty && guidNewInvoice.HasValue)
            {
                result.IsValid = true;
                result.ItemResponse = guidNewInvoice;
            }
            else
            {
                result.IsValid = false;
                result.ValidationMessages.Add("Lỗi khi xử lý kết quả từ store.");
            }

            return result;
        }


        public async Task<ModelResponse> PayInvoiceAsync(InvoicePaymentRequest request)
        {
            var result = new ModelResponse();

            // 1. Kiểm tra hóa đơn tồn tại
            var invoice = await _clientSupabase
                .From<InvoicesModel>()
                .Where(i => i.Invoice_Id == request.InvoiceId)
                .Single();

            if (invoice == null)
            {
                result.IsValid = false;
                result.ValidationMessages.Add("Không tìm thấy hóa đơn.");
                return result;
            }

            // 2. Kiểm tra đã thanh toán chưa
            if (!string.IsNullOrEmpty(invoice.Payment_Method))
            {
                result.IsValid = false;
                result.ValidationMessages.Add("Hóa đơn này đã được thanh toán.");
                return result;
            }

            // 3. Cập nhật trạng thái thanh toán
            invoice.Payment_Method = request.PaymentMethod;
            invoice.Payment_Time = DateTime.Now;
            invoice.Invoice_Status = true; // Gọi là đã thanh toán

            await _clientSupabase
                .From<InvoicesModel>()
                .Update(invoice);

            result.IsValid = true;
            result.ValidationMessages.Add("Thanh toán thành công.");
            return result;
        }

    }
}

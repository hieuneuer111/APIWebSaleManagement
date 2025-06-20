using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Invoices
{
    public interface IInvoiceService
    {
        Task<ModelResponse> PayInvoiceAsync(InvoicePaymentRequest request);
        Task<ModelDataResponse<Guid?>> CreateInvoiceAsync(InvoiceRequest request);
        Task<ModelDataResponse<List<SP_GetProductByInvoiceResponce>>> GetProductByInvoiceId(Guid invoiceId);
        Task<ModelDataResponse<InvoiceDetailResponse>> GetInvoiceById(Guid invoiceId);
        Task<ModelDataPageResponse<List<InvoiceListResponse>>> GetInvoiceList(DateTime? dateStart, DateTime? dateEnd, List<string> userId, int PageNumber, int PageSize, bool isPaging, bool isDescendPrice, int isStatus);
    }
}

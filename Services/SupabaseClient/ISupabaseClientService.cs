using Supabase;
using Supabase.Gotrue;
using WebAPISalesManagement.ModelResponses;

namespace WebAPISalesManagement.Services.SupabaseClient
{
    public interface ISupabaseClientService
    {
        Supabase.Client GetSupabaseClient();
        Task<SP_GetUserByUNameResponse> GetUserByUsernameAsync(string username);
        Task<List<SP_GetRightByUidRightIdResponse>> GetRightByRoleIdAsync(string roleId);
        Task<List<SP_GetRightByUidRightIdResponse>> GetRightByUIdAsync(Guid userIdResquest);
        Task<List<SP_GetFilesByFolderResponse>> GetFileByFolderAsync(string folderName);
        Task<SP_DeleteAllFileInFolderResponse> DeleteAllFileInFolder(string folder);
        Task<List<SP_GetProductByInvoiceResponce>> GetProductByInvoice(Guid idInvoices);
        Task<List<SP_GetDiscountByInvoiceResponse>> GetDiscountByInvoice(Guid idInvoices);
        Task<Guid?> CreateInvoice(object invoiceJson);

        Task<List<SP_BestSellingProductResponse>> GetBestSellingProduct(DateTime? dateFrom, DateTime? dateEnd, int top);
    }
}

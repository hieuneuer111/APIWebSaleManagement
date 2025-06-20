using Newtonsoft.Json;
using Supabase;
using Supabase.Gotrue;
using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Services.Configuration;
using System.Text.Json;
using System.Collections.Generic;
using WebAPISalesManagement.Swagger;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace WebAPISalesManagement.Services.SupabaseClient
{
    public class SupabaseClientService : ISupabaseClientService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly IConfigService _configService;
        public SupabaseClientService(Supabase.Client supabaseClient, IConfigService configService)
        {
            _supabaseClient = supabaseClient;
            _configService = configService;
        }
        public Supabase.Client GetSupabaseClient()
        {
            return _supabaseClient;
        }
        // Hàm gọi stored procedure getUserByUsername
        public async Task<SP_GetUserByUNameResponse> GetUserByUsernameAsync(string username)
        {
            try
            {
                var response = await _supabaseClient
                .Rpc(StoreProcedureSupabase.GetUserByUsername, new { uname = (username) });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_GetUserByUNameResponse> users = JsonConvert.DeserializeObject<List<SP_GetUserByUNameResponse>>(response.Content);
                        if (users.Count > 0)
                        {
                            return users[0];
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex) {
                return null;
            }
            // Gọi stored procedure getUserByUsername từ Supabase  
        }
        public async Task<List<SP_GetRightByUidRightIdResponse>> GetRightByRoleIdAsync(string roleIdResquest)
        {
            try
            {
                // Gọi stored procedure getUserByUsername từ Supabase
                var response = await _supabaseClient
                    .Rpc(StoreProcedureSupabase.GetRightByRoleId, new { roleid = Guid.Parse(roleIdResquest) });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_GetRightByUidRightIdResponse> rights = JsonConvert.DeserializeObject<List<SP_GetRightByUidRightIdResponse>>(response.Content);
                        if (rights.Count > 0)
                        {
                            return rights;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex) {
                return null;
            }  
        }
        public async Task<List<SP_GetRightByUidRightIdResponse>> GetRightByUIdAsync(Guid userIdResquest)
        {
            try
            {
                // Gọi stored procedure getUserByUsername từ Supabase
                var response = await _supabaseClient
                    .Rpc(StoreProcedureSupabase.GetRightByUId, new { uid = userIdResquest });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_GetRightByUidRightIdResponse> rights = JsonConvert.DeserializeObject<List<SP_GetRightByUidRightIdResponse>>(response.Content);
                        if (rights.Count > 0)
                        {
                            return rights;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<SP_GetFilesByFolderResponse>> GetFileByFolderAsync(string folderName)
        {
            try
            {
                var a = _configService.GetJWT().BucketUploadName;
                // Gọi stored procedure getUserByUsername từ Supabase
                var response = await _supabaseClient
                    .Rpc(StoreProcedureSupabase.GetFilesByFolder, new { bucket = _configService.GetJWT().BucketUploadName, prefix = folderName });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_GetFilesByFolderResponse> rights = JsonConvert.DeserializeObject<List<SP_GetFilesByFolderResponse>>(response.Content);
                        if (rights.Count > 0)
                        {
                            return rights;
                        }
                        else
                        {
                            return null;
                        }

                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SP_DeleteAllFileInFolderResponse> DeleteAllFileInFolder(string folder)
        {
            SP_DeleteAllFileInFolderResponse sp_result = new SP_DeleteAllFileInFolderResponse();
            try
            {
                string bucketName = _configService.GetJWT().BucketUploadName;
                Supabase.Postgrest.Responses.BaseResponse response = await _supabaseClient
                    .Rpc(StoreProcedureSupabase.DeleteAllFilesByFolder, new { bucket = bucketName, prefix = folder });

                if (response != null && response.Content != null)
                {
                    sp_result = JsonConvert.DeserializeObject<SP_DeleteAllFileInFolderResponse>(response.Content.ToString());
                }
                else
                {
                    sp_result = new SP_DeleteAllFileInFolderResponse
                    {
                        AllDeleted = false,
                        DeletedList = new List<DeletedFileResponse>(),
                        ErrorMessage = "No response from Supabase RPC."
                    };
                }
            }
            catch (Exception ex)
            {
                sp_result = new SP_DeleteAllFileInFolderResponse
                {
                    AllDeleted = false,
                    DeletedList = new List<DeletedFileResponse>(),
                    ErrorMessage = ex.Message
                };
            }

            return sp_result;
        }
        public async Task<List<SP_GetProductByInvoiceResponce>> GetProductByInvoice(Guid idInvoices)
        {
            try
            {
                // Gọi stored procedure getUserByUsername từ Supabase
                var response = await _supabaseClient
                    .Rpc(StoreProcedureSupabase.GetProductByInvoices, new { invoiceid = idInvoices });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_GetProductByInvoiceResponce> result = JsonConvert.DeserializeObject<List<SP_GetProductByInvoiceResponce>>(response.Content);
                        if (result.Count > 0)
                        {
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<SP_GetDiscountByInvoiceResponse>> GetDiscountByInvoice(Guid idInvoices)
        {
            try
            {
                // Gọi stored procedure getUserByUsername từ Supabase
                var response = await _supabaseClient
                    .Rpc(StoreProcedureSupabase.GetDiscountsByInvoices, new { invoiceid = idInvoices });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_GetDiscountByInvoiceResponse>? result = JsonConvert.DeserializeObject<List<SP_GetDiscountByInvoiceResponse>>(response.Content);
                        if (result?.Count > 0)
                        {
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Guid?> CreateInvoice(object invoiceJson)
        {
            try
            {               
                // Gọi stored procedure getUserByUsername từ Supabase               
                var response = await _supabaseClient
               .Rpc(StoreProcedureSupabase.CreateInvoices, new Dictionary<string, object>
               {
                    { "input", invoiceJson }
               });
                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content

                    var raw = response.Content.Trim('"'); // ← Bỏ dấu ngoặc kép
                    if (Guid.TryParse(raw, out var parsedGuid))
                    {
                        return parsedGuid;
                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DateTime? convertDateTimeToNull(DateTime? dateTime)
        {
            DateTime? converter = null;
            if (dateTime.HasValue)
            {
                converter = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day);
            }
            return converter;
        }
        public async Task<List<SP_BestSellingProductResponse>> GetBestSellingProduct(DateTime? dateFrom, DateTime? dateEnd, int top)
        {
            try
            {
                DateTime? dateF= dateFrom ?? null;
                // Gọi stored procedure getUserByUsername từ Supabase
                var response = await _supabaseClient
                .Rpc(StoreProcedureSupabase.GetBestSellingProducts, new Dictionary<string, object>
                {
                    { "start_date", convertDateTimeToNull(dateFrom) ?? null },
                    { "end_date", convertDateTimeToNull(dateEnd)?? null },
                    { "top_limit", top }
                });

                // Kiểm tra mã trạng thái của phản hồi
                if (response.ResponseMessage?.IsSuccessStatusCode == true)
                {
                    // Nếu phản hồi thành công, lấy dữ liệu từ Content
                    if (response.Content != null || response.Content != "[]")
                    {
                        // Giả sử Content là JSON, hãy deserialize nó thành kiểu User
                        //var user = JsonConvert.DeserializeObject<User>(response.Content);
                        List<SP_BestSellingProductResponse>? result = JsonConvert.DeserializeObject<List<SP_BestSellingProductResponse>>(response.Content);
                        if (result?.Count > 0)
                        {
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    // Nếu không thành công, in ra thông báo lỗi
                    return null;
                }
                // Nếu có lỗi hoặc không có dữ liệu
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
    }
}

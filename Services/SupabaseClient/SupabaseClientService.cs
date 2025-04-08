using Newtonsoft.Json;
using Supabase;
using Supabase.Gotrue;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Services.Configuration;

namespace WebAPISalesManagement.Services.SupabaseClient
{
    public class SupabaseClientService : ISupabaseClientService
    {
        private readonly Supabase.Client _supabaseClient;
        public SupabaseClientService(Supabase.Client supabaseClient)
        {
           _supabaseClient = supabaseClient;
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

       
      
    }
}

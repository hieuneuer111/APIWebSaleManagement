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
        
      
    }
}

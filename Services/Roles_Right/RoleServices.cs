using Microsoft.AspNetCore.Identity.Data;
using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Roles
{
    public class RoleServices : IRoleServices
    {
        private readonly Supabase.Client _clientSupabase;
        private readonly ISupabaseClientService _supabaseClientService;
        public RoleServices (Supabase.Client client, ISupabaseClientService supabaseClientService)
        {
            _clientSupabase = client;
            _supabaseClientService = supabaseClientService;
        }

        public async Task<ModelDataResponse<List<RightResponse>>> GetRightByRoleIdAsync(string roleId)
        {
            ModelDataResponse<List<RightResponse>> result = new ModelDataResponse<List<RightResponse>>();
            List<SP_GetRightByUidRightIdResponse> rightsSP = await _supabaseClientService.GetRightByRoleIdAsync(roleId);
            List<RightResponse> rights = rightsSP.Select(u => new RightResponse
            {
                RightId = u.right_id_pro,
                RightName = u.right_name_pro,
                Description = u.right_description,
            }).ToList();
            result.ItemResponse = rights;
            return result;
        }

        public Task<ModelDataResponse<List<RolesResponse>>> GetRolesAsync(string? search)
        {
            throw new NotImplementedException();
        }

        public async Task<RolesResponse> GetRolesByIDAsync(Guid roleID)
        {
            ModeledResponse<RolesModel> SupabaseResponse = await _clientSupabase.From<RolesModel>().Where(u => u.Role_Id == roleID).Get();
            RolesModel rolesModel = SupabaseResponse.Models.FirstOrDefault();
            RolesResponse roles = new RolesResponse()
            {
                RoleId = rolesModel.Role_Id,
                RoleName = rolesModel.Role_Name,
                Description = rolesModel.Description,
            };
            return roles;
        }
    }
}

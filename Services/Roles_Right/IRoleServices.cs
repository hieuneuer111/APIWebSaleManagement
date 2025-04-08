
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Roles
{
    public interface IRoleServices
    {
        Task<ModelDataResponse<List<RolesResponse>>> GetRolesAsync(string? search);
        Task<ModelDataResponse<List<RightResponse>>> GetRightByRoleIdAsync(string roleId);
        Task<RolesResponse> GetRolesByIDAsync(Guid roleID);
    }
}

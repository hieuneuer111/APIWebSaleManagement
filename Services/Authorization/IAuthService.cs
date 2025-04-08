using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPISalesManagement.ModelRequests;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Authorization
{
    public interface IAuthService
    {
        Task<Response<SupabaseResponse>> LoginJWTAsync(LoginSupabaseRequest loginRequest);
        Task<Response<SupabaseUserResponse>> Register(UserRegisterResquest userLogin);
        Task<Response<SupabaseResponse>> ReloadByRefreshToken(string refreshToken);
        Task<bool> VerifyEmailAsync(Guid idUser);
    }
}

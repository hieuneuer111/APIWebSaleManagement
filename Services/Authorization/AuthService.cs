using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelRequests;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.Configuration;
using WebAPISalesManagement.Services.Roles;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Settings;
using WebAPISalesManagement.Swagger;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace WebAPISalesManagement.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly ISupabaseClientService _supabaseClientService;
        private readonly IRoleServices _roleServices;
        private readonly Supabase.Client _supabaseClient;
       
        private readonly WebAPISalesManagement.Services.Configuration.IConfigurationService  _configurationService;
        public AuthService(ISupabaseClientService supabaseClientService, WebAPISalesManagement.Services.Configuration.IConfigurationService configurationService, Supabase.Client supabaseClient, IRoleServices roleServices)
        {
        
            _supabaseClientService = supabaseClientService;
            _configurationService = configurationService;
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient)); ;
            _roleServices = roleServices;
           

        }
        // Đăng nhập vào Supabase bằng email và password
        private async Task<SupabaseResponse> AuthenticateWithSupabase(string email, string password, Guid rolesResponse, Guid uid)
        {
            SupabaseResponse userInfo = new SupabaseResponse();
            try
            {
                Supabase.Gotrue.Session responseAuth = await _supabaseClient.Auth.SignIn(email, password);
                if (responseAuth != null)
                {
                    await VerifyEmailAsync(Guid.Parse(responseAuth.User.Id));
                    RolesResponse roleUser = await _roleServices.GetRolesByIDAsync(rolesResponse);
                    List<SP_GetRightByUidRightIdResponse> rights_SP= await _supabaseClientService.GetRightByUIdAsync(Guid.Parse(responseAuth.User.Id));
                    userInfo = new SupabaseResponse
                    {
                        AccessToken = responseAuth.AccessToken,
                        ExpiresIn = responseAuth.ExpiresIn,
                        RefreshToken = responseAuth.RefreshToken,
                        User = new SupabaseUserResponse
                        {
                            Id = Guid.Parse(responseAuth.User.Id),
                            Email = responseAuth.User.Email,
                            Role = roleUser,
                            Userrights = rights_SP
                        },
                    };
                }
            }
            catch (Exception ex) {
                // Kiểm tra lỗi từ GotrueException
                //{ "code":400,"error_code":"email_not_confirmed","msg":"Email not confirmed"}
                //{ "code":400,"error_code":"invalid_credentials","msg":"Invalid login credentials"}
                // Parse chuỗi JSON
                JObject jsonObject = JObject.Parse(ex.Message);

                string msg = jsonObject["msg"]?.ToString();
                userInfo.MsgError = msg;
            }
            return userInfo;
        }
        public async Task<Response<SupabaseResponse>> LoginJWTAsync(LoginSupabaseRequest loginRequest)
        {
            Response<SupabaseResponse> result = new Swagger.Response<SupabaseResponse>();
            List<RightResponse> rightsUser = new List<RightResponse> { };
            /// // Bước 1: Tìm user trong bảng Users bằng username
            SP_GetUserByUNameResponse user = await _supabaseClientService.GetUserByUsernameAsync(loginRequest.UserName);
           
            if (user == null)
            {
                result.Succeeded = false;
                result.Errors = [];
                result.Message = "Username not found.";
                result.Data = null;
                return result;
            }
            // Bước 2: Đăng nhập với Supabase bằng email và password
            SupabaseResponse account = await AuthenticateWithSupabase(user.EmailUser, loginRequest.Password, user.rolesUser, user.Uid);
            if (!string.IsNullOrWhiteSpace(account.MsgError))
            {
                result.Succeeded = false;
                result.Errors = [];
                result.Message = account.MsgError;
                result.Data = null;
                return result;
            }
           
            result.Succeeded = true;
            result.Errors = [];
            result.Message = "Login Success!";
            result.Data = account;
            return result;
        }
        public async Task<Response<SupabaseUserResponse>> Register(UserRegisterResquest userLogin)
        {
            try
            {
                Response<SupabaseUserResponse> result = new Swagger.Response<SupabaseUserResponse>();
                // Đăng ký tài khoản người dùng
                Supabase.Gotrue.Session response = await _supabaseClient.Auth.SignUp(userLogin.Email, userLogin.Password);
                if (response == null)
                {
                    result.Succeeded = false;
                    result.Errors = [];
                    result.Message = "System API Errors";
                    result.Data = null;                  
                }
                else
                {
                    if (response.User != null)
                    {
                        // Kiểm tra xem người dùng có đăng ký thành công không
                        if (response.User.EmailConfirmedAt == null)
                        {
                            // Thực hiện hành động bạn muốn khi người dùng mới đăng ký
                            UsersModel userModel = new UsersModel()
                            {
                                Email = userLogin.Email,                               
                                FullName = userLogin.FullName,
                                Phone = userLogin.Phone,
                                Role_id = userLogin.RoleId,
                                Status = userLogin.Status,
                                User_Number = userLogin.Usernumber,
                                Username = userLogin.UserName,
                                User_id = Guid.Parse(response.User.Id),
                                //"70db78df-18d0-47ee-bf90-574424e59d76"
                            };
                            ModeledResponse<UsersModel> addResponse = await _supabaseClient.From<UsersModel>().Insert(userModel);
                            UsersModel userRegisterReponse = addResponse?.Models.FirstOrDefault();
                            if (userRegisterReponse != null)
                            {
                                RolesResponse roleUser = await _roleServices.GetRolesByIDAsync(userLogin.RoleId);
                                List<SP_GetRightByUidRightIdResponse> rights_SP = await _supabaseClientService.GetRightByUIdAsync(userRegisterReponse.User_id);
                                result.Succeeded = true;
                                result.Errors = [];
                                result.Message = "Register Success!";
                                result.Data = new SupabaseUserResponse
                                {
                                    Id = userRegisterReponse.User_id,
                                    Email = userRegisterReponse.Email,
                                    Role = roleUser,
                                    Userrights = rights_SP
                                };
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<Response<SupabaseResponse>> ReloadByRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> VerifyEmailAsync(Guid idUser) {
            ModeledResponse<UsersModel> updateResponse = await _supabaseClient
                              .From<UsersModel>()
                              .Where(x => x.User_id == idUser)
                              .Set(x => x.Verify_email, true)
                              .Update();
            if (updateResponse == null || updateResponse.Models.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
           
        }
        
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using Supabase.Interfaces;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Swagger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPISalesManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
       
        private readonly WebAPISalesManagement.Services.Authorization.IAuthService _authorizationService;
        public AuthorizationController(WebAPISalesManagement.Services.Authorization.IAuthService authorizationService) {
            _authorizationService = authorizationService;
        }
        // GET: api/<AuthorizationController>


        // POST api/<AuthorizationController>
        /// <summary>
        /// Đăng nhập lấy token JWT
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("LoginAuthorizationJWT")]
        public async Task<ActionResult> LoginJWTAsync ([FromBody] ModelRequests.LoginSupabaseRequest request)
        {
            try
            {
                Response<SupabaseResponse> result = await _authorizationService.LoginJWTAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // POST api/<AuthorizationController>
        /// <summary>
        /// Đăng kí tài khoản bắt buộc có email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RegisterAuthorizationJWT")]
        public async Task<ActionResult> RegisterAuthorizationJWT([FromBody] ModelRequests.UserRegisterResquest request)
        {
            try
            {
                Response<SupabaseUserResponse> result = await _authorizationService.Register(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //
        /// <summary>
        /// Reload Auto login by Refresh Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost("ReloadByRefreshToken")]
        public async Task<ActionResult> ReloadByRefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                Response<SupabaseResponse> result = await _authorizationService.ReloadByRefreshToken(request.RefreshToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Logout 
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authorizationService.Logout();
                // (Tuỳ chọn) Xoá refresh token lưu trong DB nếu bạn lưu
                // await _refreshTokenService.DeleteAsync(userId);
                return Ok(new { message = "Đăng xuất thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi khi đăng xuất", details = ex.Message });
            }
        }
    }
}

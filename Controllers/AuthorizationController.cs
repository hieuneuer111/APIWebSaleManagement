using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supabase.Gotrue;
using WebAPISalesManagement.ModelResponses;
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
        [HttpPost("LoginAuthorizationJWT")]
        public async Task<ActionResult> LoginJWTAsync ([FromBody] ModelRequests.LoginSupabaseRequest request)
        {
            Response<SupabaseResponse> result = await _authorizationService.LoginJWTAsync(request);
            return Ok(result);
        }
        // POST api/<AuthorizationController>
        [HttpPost("RegisterAuthorizationJWT")]
        public async Task<ActionResult> RegisterAuthorizationJWT([FromBody] ModelRequests.UserRegisterResquest request)
        {
            Response<SupabaseUserResponse> result = await _authorizationService.Register(request);
            return Ok(result);
        }
        // 
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebAPISalesManagement.Helpers
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeRightAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredRight;

        public AuthorizeRightAttribute(string requiredRight)
        {
            _requiredRight = requiredRight;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Lấy access token từ Header
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Lấy claim "user_metadata" từ token
                var metadataClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_metadata");
                if (metadataClaim == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                var metadataJson = metadataClaim.Value;
                var metadata = System.Text.Json.JsonDocument.Parse(metadataJson);
                var rights = metadata.RootElement.GetProperty("userrights");

                // Kiểm tra xem quyền yêu cầu có tồn tại trong userrights không
                var hasRight = rights.EnumerateArray().Any(r =>
                    r.TryGetProperty("right_name_pro", out var nameProp) &&
                    nameProp.GetString() == _requiredRight);

                if (!hasRight)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

}

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPISalesManagement.Helpers
{
    public class JwtMetadataMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMetadataMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // Lấy user_metadata từ Supabase token
                var metadata = jwt.Claims.FirstOrDefault(c => c.Type == "user_metadata")?.Value;

                if (!string.IsNullOrEmpty(metadata))
                {
                    var metadataObj = JsonConvert.DeserializeObject<JObject>(metadata);

                    // Gắn metadata vào HttpContext.User (tùy bạn lưu ở đâu)
                    var identity = new ClaimsIdentity();
                    identity.AddClaim(new Claim("userrights", metadataObj["userrights"]?.ToString() ?? "[]"));

                    var principal = new ClaimsPrincipal(identity);
                    context.User = principal;
                }
            }

            await _next(context);
        }
    }

}

using WebAPISalesManagement.Settings;
using Microsoft.Extensions.Configuration;

namespace WebAPISalesManagement.Services.Configuration
{
    public class ConfigurationService:IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JWT GetJWT()
        {
            JWT jWT = new JWT()
            {
                JWTSecret = _configuration["Authentication:JWTSecret"],
                SUPABASE_KEY = _configuration["Authentication:SUPABASE_KEY"],
                SUPABASE_URL = _configuration["Authentication:SUPABASE_URL"],
                ValidAudience = _configuration["Authentication:ValidAudience"],
                ValidIsuser = _configuration["Authentication:ValidIsuser"]
            };
            return jWT;
        }

      

    }
}

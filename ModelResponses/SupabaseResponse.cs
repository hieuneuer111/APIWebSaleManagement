using Newtonsoft.Json;

namespace WebAPISalesManagement.ModelResponses
{
    public class SupabaseResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("user")]
        public SupabaseUserResponse User { get; set; }
        public string? MsgError { get; set; }
    }
}

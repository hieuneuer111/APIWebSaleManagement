using Newtonsoft.Json;

namespace WebAPISalesManagement.ModelResponses
{
    public class SupabaseUserResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public RolesResponse Role { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        //
        public List<SP_GetRightByUidRightIdResponse> Userrights { get; set; }

    }
}

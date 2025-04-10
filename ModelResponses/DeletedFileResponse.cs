using Newtonsoft.Json;

namespace WebAPISalesManagement.ModelResponses
{
    public class DeletedFileResponse
    {
        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("mimetype")]
        public string MimeType { get; set; }
    }
}

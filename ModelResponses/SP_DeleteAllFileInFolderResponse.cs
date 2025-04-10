using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebAPISalesManagement.ModelResponses
{
    public class SP_DeleteAllFileInFolderResponse
    {
        [JsonProperty("all_deleted")]
        public bool AllDeleted { get; set; }

        [JsonProperty("deletedList")]
        public List<DeletedFileResponse> DeletedList { get; set; }

        [JsonProperty("folder_existed")]
        public bool FolderExisted { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}

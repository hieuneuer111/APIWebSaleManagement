using System.Numerics;

namespace WebAPISalesManagement.ModelResponses
{
    public class SP_GetFilesByFolderResponse
    {
        public string path { get; set; }
        public string file_name { get; set; }
        public long size { get; set; }
        public string mimetype { get; set; }
        public long content_length { get; set; }
        public DateTime last_modified { get; set; }
    }
}

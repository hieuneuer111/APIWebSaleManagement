using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, string fileName, bool UpdateUrlImgToProduct);
        Task<ModelResponse> DeleteFilesInFolderAsync(string urlProduct);
        Task<ModelDataResponse<List<SP_GetFilesByFolderResponse>>> GetFileByFolderSupabase(string folderName);
        Task<ModelResponse> UpdateFileByFolder(string folderName, IFormFile file);
        Task<ModelResponse> DeleteFolderByNameAsync(string folderName);
    }
}

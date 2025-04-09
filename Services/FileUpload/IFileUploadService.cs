using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName, string fileName, bool UpdateUrlImgToProduct);
        Task<ModelResponse> DeleteFilesInFolderAsync(string urlProduct);
    }
}

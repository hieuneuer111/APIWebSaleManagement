using Microsoft.AspNetCore.Mvc;
using WebAPISalesManagement.Services.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace WebAPISalesManagement.Controllers
{
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }
        /// <summary>
        /// Upload File To Storage Supabase
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        // API endpoint để upload file
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileAsync(string folderName, IFormFile file, bool IsUpdateUrlImgToProduct = true)
        {
            if (file == null || file.Length == 0 )
            {
                return BadRequest(new {  Message = "No file uploaded.!" });
            }
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return BadRequest(new { Message = "No Folder uploaded.!" });
            }
            try
            {
                //var bucketName = "salesmanagementproduct"; // Đặt tên bucket của bạn ở đây
                var fileName = file.FileName; // Sử dụng tên file gốc, có thể thay đổi nếu cần
                // Gọi service upload file
                var fileUrl = await _fileUploadService.UploadFileAsync(file, folderName, fileName, IsUpdateUrlImgToProduct);
                return Ok(new { FileUrl = fileUrl, Message ="Upload Success!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error uploading file", Details = ex.Message });
            }
        }
        /// <summary>
        /// Xóa tất cả file trong folder SUPABASE 
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [HttpDelete("DeleteFolder")]
        public async Task<IActionResult> DeleteFolderAsync(string urlImgDelete)
        {
            try
            {
                // Gọi service để xóa tất cả các tệp trong thư mục
                await _fileUploadService.DeleteFilesInFolderAsync(urlImgDelete);

                return Ok(new { Message = "Folder and its contents deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting folder", Details = ex.Message });
            }
        }
    }
}

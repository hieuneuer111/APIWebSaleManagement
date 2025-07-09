using Microsoft.AspNetCore.Mvc;
using WebAPISalesManagement.Services.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Swagger;
using WebAPISalesManagement.Services.SupabaseClient;
using Microsoft.AspNetCore.Authorization;
namespace WebAPISalesManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly ISupabaseClientService _supabaseClientService;

        public FileUploadController(IFileUploadService fileUploadService, ISupabaseClientService supabaseClientService)
        {
            _fileUploadService = fileUploadService;
            _supabaseClientService = supabaseClientService;
        }
        /// <summary>
        /// Upload File To Storage Supabase
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        // API endpoint để upload file
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFileAsync(string IdProduct, IFormFile file, bool IsUpdateUrlImgToProduct = true)
        {
            if (file == null || file.Length == 0 )
            {
                return BadRequest(new {  Message = "No file uploaded.!" });
            }
            if (string.IsNullOrWhiteSpace(IdProduct))
            {
                return BadRequest(new { Message = "No Folder uploaded.!" });
            }
            try
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                //var bucketName = "salesmanagementproduct"; // Đặt tên bucket của bạn ở đây
                //var fileName = file.FileName; // Sử dụng tên file gốc, có thể thay đổi nếu cần
                // Gọi service upload file
                var fileUrl = await _fileUploadService.UploadFileAsync(file, IdProduct, uniqueFileName, IsUpdateUrlImgToProduct);
                return Ok(new { FileUrl = fileUrl, Message ="Upload Success!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error uploading file", Details = ex.Message });
            }
        }

        /// <summary>
        /// Update file To Storage Supabase
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="file"></param>
        /// <param name="IsUpdateUrlImgToProduct"></param>
        /// <returns></returns>
        [HttpPost("UpdateFile")]
        public async Task<IActionResult> UpdateFile(string folderName, IFormFile file, bool IsUpdateUrlImgToProduct = true)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "No file uploaded.!" });
            }
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return BadRequest(new { Message = "No Folder uploaded.!" });
            }
            try
            {
                //var bucketName = "salesmanagementproduct"; // Đặt tên bucket của bạn ở đây
                //string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}"; // Sử dụng tên file gốc, có thể thay đổi nếu cần
                // Gọi service upload file
                ModelResponse fileUrl = await _fileUploadService.UpdateFileByFolder(folderName, file);
                return Ok(fileUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error uploading file", Details = ex.Message });
            }
        }
        /// <summary>
        /// Get files in folder Supabase
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [HttpGet("GetFileByFolder")]
        public async Task<IActionResult> GetFileByFolder(string folderName)
        {
            try
            {
                // Gọi service để xóa tất cả các tệp trong thư mục
                ModelDataResponse<List<SP_GetFilesByFolderResponse>> response = await _fileUploadService.GetFileByFolderSupabase(folderName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting folder", Details = ex.Message });
            }
        }
        /// <summary>
        /// Xóa 1 file trong folder SUPABASE dựa vào URL Image
        /// </summary>
        /// <param name="urlImgDelete"></param>
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
        /// <summary>
        /// Delete all folder using store produce
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [HttpDelete("DeleteAllFileFolder")]
        public async Task<IActionResult> DeleteAllFileFolder(string folderName)
        {
            try
            {
                // Gọi service để xóa tất cả các tệp trong thư mục
                SP_DeleteAllFileInFolderResponse sP_DeleteAllFileInFolderResponse = await _supabaseClientService.DeleteAllFileInFolder(folderName);
                return Ok(sP_DeleteAllFileInFolderResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting folder", Details = ex.Message });
            }
        }

    }
}

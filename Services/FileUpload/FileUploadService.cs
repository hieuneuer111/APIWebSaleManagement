using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;
using Supabase;
using System.Net.Http;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Services.Configuration;
using Supabase.Interfaces;
using WebAPISalesManagement.Swagger;
using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Services.Products;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.SupabaseClient;

namespace WebAPISalesManagement.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigService _configuration;
        private readonly string supabaseUrl; // Lấy URL Supabase từ appsettings.json
        private readonly string supabaseApiKey; // Lấy API Key Supabase từ appsettings.json
        private readonly string bucketName;
        private readonly Supabase.Client _supabaseClient;
        private readonly ISupabaseClientService _supabaseClientService;

        public FileUploadService(HttpClient httpClient, IConfigService configuration, Supabase.Client supabaseClient, ISupabaseClientService supabaseClientService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
             supabaseUrl = _configuration.GetJWT().SUPABASE_URL; // Lấy URL Supabase từ appsettings.json
             supabaseApiKey = _configuration.GetJWT().SUPABASE_KEY; // Lấy API Key Supabase từ appsettings.json
             bucketName = _configuration.GetJWT().BucketUploadName;
            _supabaseClient = supabaseClient;
            _supabaseClientService = supabaseClientService;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName, string fileName, bool UpdateUrlImgToProduct)
        {
            try
            {                                                       // Cập nhật Key để chỉ định thư mục
                string filePath = $"{folderName}/{fileName}"; // Thư mục + tên file
                string url = $"{supabaseUrl}/storage/v1/object/{bucketName}/{filePath}";
                using (var formData = new MultipartFormDataContent())
                {
                    using (var fileStream = file.OpenReadStream())
                    {
                        StreamContent fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        formData.Add(fileContent, "file", fileName);

                        // Thêm header Authorization với Bearer token
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", supabaseApiKey);

                        // Gửi yêu cầu POST lên Supabase Storage
                        HttpResponseMessage response = await _httpClient.PostAsync(url, formData);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            UploadSupabaseResponse responseObject = System.Text.Json.JsonSerializer.Deserialize<UploadSupabaseResponse>(responseContent);

                            // Tạo URL đầy đủ của file sau khi upload
                            string fileUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{filePath}";
                            if (UpdateUrlImgToProduct)
                            {
                                Guid id = Guid.Parse(folderName);
                                if (id != Guid.Empty)
                                {
                                    ModeledResponse<ProductsModel> updateResponse = await _supabaseClient
                                                      .From<ProductsModel>()
                                                      .Where(x => x.Product_Id == id)
                                                      .Set(x => x.Product_ImgURL, fileUrl)
                                                      .Update();
                                }
                            }
                            return fileUrl;
                        }
                        else
                        {
                            string errorMessage = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Upload failed: {errorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file", ex);
            }
        }
        public async Task<ModelResponse> DeleteFilesInFolderAsync(string urlProduct)
        {
            ModelResponse modelResponse = new ModelResponse();
            try
            {             
                //string deleteUrl1 = $"{supabaseUrl}/storage/v1/object/{bucketName}/"+ "DL1/Screenshot%202024-12-09%20083358.png";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", supabaseApiKey);
                _httpClient.DefaultRequestHeaders.Add("apikey", supabaseApiKey);
                HttpResponseMessage deleteResponse1 = await _httpClient.DeleteAsync(urlProduct);
                if (deleteResponse1.IsSuccessStatusCode)
                {
                    modelResponse.IsValid = true;
                    modelResponse.ValidationMessages.Add("File deleted successfully.");
                }
                else
                {
                    string errorMessage = await deleteResponse1.Content.ReadAsStringAsync();
                    modelResponse.IsValid = false;
                    modelResponse.ValidationMessages.Add(errorMessage);
                }
            }
            catch (Exception ex)
            {              
                modelResponse.IsValid = false;
                modelResponse.ValidationMessages.Add("Error deleting files in folder" + ex.Message);
            }
            return modelResponse;
        }
        public async Task<ModelDataResponse<List<SP_GetFilesByFolderResponse>>> GetFileByFolderSupabase(string folderName)
        {
            ModelDataResponse<List<SP_GetFilesByFolderResponse>> modelResponse = new ModelDataResponse<List<SP_GetFilesByFolderResponse>>();
            try
            {
                List<SP_GetFilesByFolderResponse> filesList = await _supabaseClientService.GetFileByFolderAsync(folderName);
                modelResponse.IsValid = true;
                modelResponse.ItemResponse = filesList;
            }
            catch (Exception ex) {
                modelResponse.IsValid = false;
                modelResponse.ItemResponse = null;
                modelResponse.ValidationMessages.Add(ex.Message);
            }
            return modelResponse;
        }
        public async Task<ModelResponse> UpdateFileByFolder(string folderName, IFormFile file)
        {
            ModelResponse modelResponse = new ModelResponse();
            /// Xóa file trong thư mục cũ         
            SP_DeleteAllFileInFolderResponse responseDeleteFolder = await _supabaseClientService.DeleteAllFileInFolder(folderName);
            if (responseDeleteFolder.AllDeleted == true || responseDeleteFolder.FolderExisted == false) {
                /// đã xóa thành công tạo mới
                string urlImg = await UploadFileAsync(file, folderName, file.FileName, true);
                /// 
            }
            ///////////////////
            return modelResponse;   
        }
    }
}

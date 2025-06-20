using System.Net.Http.Headers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Services.Configuration;
using WebAPISalesManagement.Swagger;
using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Models;
using Supabase.Interfaces;
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

        public async Task<string> UploadFileAsync(IFormFile file, string productId, string fileName, bool UpdateUrlImgToProduct)
        {
           
            try
            {
                if (await CheckProductId(productId))
                {

                    // Cập nhật Key để chỉ định thư mục
                    string filePath = $"{productId}/{fileName}"; // Thư mục + tên file
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
                                    Guid id = Guid.Parse(productId);
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
                else {
                    throw new Exception($"Upload failed: No product");
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
            Guid guid = Guid.Parse(folderName);
            ModelResponse modelResponse = new ModelResponse();
            ModeledResponse<ProductsModel> supabaseResponseProduct = await _supabaseClient
               .From<ProductsModel>()
               .Where(c => c.Product_Id == guid)  // Sử dụng điều kiện để lọc theo categoryID
               .Get();
            if (supabaseResponseProduct.Models != null && supabaseResponseProduct.Models.Any())
            {
                ProductsModel model = supabaseResponseProduct.Models.FirstOrDefault();
                // Nếu có ảnh cũ => xóa trước
                if (!string.IsNullOrWhiteSpace(model.Product_ImgURL))
                {
                    string? oldFilePath = ExtractStoragePathFromUrl(model.Product_ImgURL);
                    if (!string.IsNullOrWhiteSpace(oldFilePath))
                    {
                        await DeleteFile(oldFilePath);
                    }
                }

                // Tạo tên file mới
                string newFileName = $"{Guid.NewGuid()}_{file.FileName}";
                // Upload ảnh mới
                string urlImg = await UploadFileAsync(file, folderName, newFileName, true);

                if (!string.IsNullOrWhiteSpace(urlImg))
                {
                    modelResponse.IsValid = true;
                    modelResponse.ValidationMessages.Add("Update file success!!");
                    modelResponse.ValidationMessages.Add("URL: " + urlImg);
                }
                else
                {
                    modelResponse.IsValid = false;
                    modelResponse.ValidationMessages.Add("Update failed, no URL returned.");
                }
            }
            return modelResponse;
        }

        public async Task<ModelResponse> DeleteFolderByNameAsync(string folderName)
        {
            ModelResponse response = new ModelResponse(); 
            // Thay bằng bucket bạn dùng
            string bucket = _configuration.GetJWT().BucketUploadName;

            if (string.IsNullOrWhiteSpace(folderName))
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Folder name is empty.");
                return response;
            }

            try
            {
                // List tất cả file trong folder
                var filesInFolder = await _supabaseClient.Storage
                    .From(bucket)
                    .List(folderName);

                if (filesInFolder == null || filesInFolder.Count == 0)
                {
                    response.IsValid = true;
                    response.ValidationMessages.Add("Folder is already empty or not found.");
                    return response;
                }

                // Tạo danh sách đường dẫn file để xóa
                var filePaths = filesInFolder
                    .Select(f => $"{folderName}/{f.Name}")
                    .ToList();

                // Gọi Supabase xóa file
                await _supabaseClient.Storage.From(bucket).Remove(filePaths);

                response.IsValid = true;
                response.ValidationMessages.Add("All files in folder deleted successfully.");
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"Error deleting folder: {ex.Message}");
            }

            return response;
        }

        private string? ExtractStoragePathFromUrl(string fullUrl)
        {
            try
            {
                var uri = new Uri(fullUrl);
                var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

                // Đảm bảo đường dẫn đúng định dạng: /storage/v1/object/{bucket}/{...path}
                if (segments.Length >= 5 && segments[0] == "storage" && segments[1] == "v1" && segments[2] == "object")
                {
                    // Bắt đầu từ index 4: bỏ storage/v1/object/{bucket}
                    string pathInBucket = string.Join('/', segments.Skip(4));
                    return pathInBucket;
                }
            }
            catch
            {
                // ignore errors
            }

            return null;
        }


        public async Task<bool> DeleteFile(string pathInBucket)
        {
            try
            {
                string bucketName = _configuration.GetJWT().BucketUploadName;

                var result = await _supabaseClient.Storage
                    .From(bucketName)
                    .Remove(new List<string> { pathInBucket });

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần
                return false;
            }
        }


        public async Task<bool> CheckProductId(string productId)
        {
            if (String.IsNullOrWhiteSpace(productId))
            {
                return false;
            }
            else
            {
                Guid guid = Guid.Parse(productId);
                // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
                ModeledResponse<ProductsModel> supabaseResponseCategory = await _supabaseClient
                    .From<ProductsModel>()
                    .Where(c => c.Product_Id == guid)  // Sử dụng điều kiện để lọc theo categoryID
                    .Get();
                if (supabaseResponseCategory.Models != null && supabaseResponseCategory.Models.Any())
                {
                    return true;
                }
                else { 
                    return false; 
                }    
            }
        }

       
    }
}

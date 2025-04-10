using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.Categories;
using WebAPISalesManagement.Services.FileUpload;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Products
{
    public class ProductServices : IProductServices
    {
        private readonly Supabase.Client _clientSupabase;
        private readonly ICategoryServices _categoryServices;
        private readonly IFileUploadService _fileUploadService;
        public ProductServices(Supabase.Client clientSupabase, ICategoryServices categoryServices, IFileUploadService fileUploadService)
        {
            _clientSupabase = clientSupabase;
            _categoryServices = categoryServices;
            _fileUploadService = fileUploadService;
        }
        public async Task<ProductResponse> GetProductById(Guid productId)
        {
            ProductResponse response = new ProductResponse();
            // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
            ModeledResponse<ProductsModel> supabaseResponseCategory = await _clientSupabase
                .From<ProductsModel>()
                .Where(c => c.Product_Id == productId)  // Sử dụng điều kiện để lọc theo categoryID
                .Get();
            // Kiểm tra xem có dữ liệu không
            if (supabaseResponseCategory.Models != null && supabaseResponseCategory.Models.Any())
            {
                // Chuyển kết quả từ SupabaseResponse thành List và gán cho response
                ProductsModel model = supabaseResponseCategory.Models.FirstOrDefault();
                CategoryResponse categoryResponse = await _categoryServices.GetCategoryByIdAsync(model.Product_Category);
                ProductResponse resultCategoriesItemResponse = new ProductResponse
                {
                    ProductId = model.Product_Id,
                    ProductName = model.Product_Name,
                    ProductPrice = model.Product_Price,
                    ProductStatus = model.Product_Status,
                    ProductDes = model.Product_Des,
                    ProductImgUrl = model.Product_ImgURL,
                    ProductCategory = new CategoryResponseDetail(categoryResponse.CategoryId, categoryResponse.CategoryName)
                  
                };
                // Gán dữ liệu vào categoriesResponse (Giả sử CategoriesResponse có một danh sách CategoriesItems)
                response = resultCategoriesItemResponse;
            }
            // Trả về response
            return response;
        }
        public async Task<ModelResponse> UpdateImgUrlProduct(string productId, string urlProduct)
        {
            ModelResponse modeledResponse = new ModelResponse();
            Guid id = Guid.Parse(productId);
            if (id != Guid.Empty)
            {
                ModeledResponse<ProductsModel> updateResponse = await _clientSupabase
                                  .From<ProductsModel>()
                                  .Where(x => x.Product_Id == id)
                                  .Set(x => x.Product_ImgURL, urlProduct)
                                  .Update();
                if (updateResponse == null || updateResponse.Models.Count <= 0)
                {
                    modeledResponse.IsValid = false;
                    modeledResponse.ValidationMessages.Add("Update Errors. Product is empty");
                }
                else
                {
                    modeledResponse.IsValid = true;
                    modeledResponse.ValidationMessages.Add("Update Success!");
                }
            }

            else {
                modeledResponse.IsValid = false;
                modeledResponse.ValidationMessages.Add("Update Errors. Product is empty");
            }
            return modeledResponse;
        }
        public async Task<ModelDataResponse<ProductResponse>> AddProductAsync(ProductResquest productResquest)
        {
            ModelDataResponse<ProductResponse> response = new ModelDataResponse<ProductResponse>();
            if (productResquest.ProductCategory == Guid.Empty)
            {
                return response;
            }
            ModeledResponse<ProductsModel> addResponse = await _clientSupabase.From<ProductsModel>().Insert(new ProductsModel
            {
                Product_Name = productResquest.ProductName,
                Product_Des = productResquest.ProductDes,
                Product_Price = productResquest.ProductPrice,
                Product_Status = productResquest.ProductStatus,
                Product_Category = productResquest.ProductCategory,
            });
            if (addResponse == null || addResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Add Errors");
            }
            else
            {
                ProductsModel ProductAdded = addResponse.Models.FirstOrDefault();
                response.ItemResponse = new ProductResponse()
                {
                    ProductId = ProductAdded.Product_Id,
                    ProductName = ProductAdded.Product_Name,                                
                };
                response.IsValid = true;
                response.ValidationMessages.Add("Add Success!");
            }
            return response;
        }
        public async Task<ModelResponse> DeleteProductItemsAsync(Guid productId)
        {
            ModelResponse response = new ModelResponse();
            ProductsModel modelToDelete = new ProductsModel { Product_Id = productId }; // Tạo mô hình muốn xóa
            ModeledResponse<ProductsModel> deleteResponse = await _clientSupabase
                .From<ProductsModel>()
                .Delete(modelToDelete);
            // await _supabaseClient.From<CategoriesItemsModel>().Where(x => x.CategoryId == categoryId).Delete();
            if (deleteResponse == null || deleteResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Delete Errors");
            }
            else
            {
                response.IsValid = true;
                ProductsModel pro = deleteResponse.Models.FirstOrDefault();
                if (pro != null)
                {
                    if (!string.IsNullOrEmpty(pro.Product_ImgURL))
                    {
                        ModelResponse result = await _fileUploadService.DeleteFilesInFolderAsync(pro.Product_ImgURL);
                        if (result.IsValid)
                        {  
                            response.ValidationMessages.Add("Delete Product And File Success!");
                        }
                        else
                        {
                            response.ValidationMessages.Add("Deleted Product But Can't Delete File!");
                        }
                    }
                    else
                    {
                        response.ValidationMessages.Add("Deleted Product Success!");
                    }
                }
                else
                {
                    response.ValidationMessages.Add("Deleted Product Success!");
                }
            }
            return response;
        }
        public async Task<ModelDataPageResponse<List<ProductResponse>>> GetProductAsync(string search, List<string> category, int PageNumber, int PageSize, bool isPaging, bool isDescendPrice)
        {
            // Lấy danh sách MenuItems từ Supabase
            ModeledResponse<ProductsModel> SupabaseResponseMenuItems = await _clientSupabase.From<ProductsModel>().Get();
            List<ProductsModel> SupabaseListMenuItems = SupabaseResponseMenuItems.Models.ToList();

            // Tìm kiếm theo tên món ăn (nếu có)
            if (!string.IsNullOrWhiteSpace(search))
            {
                StringConvert conv = new StringConvert();
                search = conv.ConvertToUnSign(search);
                SupabaseListMenuItems = SupabaseListMenuItems.Where(tb => conv.ConvertToUnSign(tb.Product_Name).Contains(search)).ToList();
            }

            // Lọc theo danh mục (nếu có)
            if (category != null)
            {
                if (category.Count > 0)
                    SupabaseListMenuItems = SupabaseListMenuItems.Where(tb => category.Contains(tb.Product_Category.ToString())).ToList();
            }

            // Sử dụng Task.WhenAll để đợi các tác vụ bất đồng bộ
            var productResponsesTasks = SupabaseListMenuItems.Select(async (ProductsModel item) =>
            {
                CategoryResponse categoryResponse = await _categoryServices.GetCategoryByIdAsync(item.Product_Category);
                return new ProductResponse
                {
                  ProductCategory = categoryResponse,
                  ProductDes = item.Product_Des,
                  ProductName = item.Product_Name,  
                  ProductId = item.Product_Id,
                  ProductImgUrl = item.Product_ImgURL,
                  ProductPrice = item.Product_Price,
                  ProductStatus = item.Product_Status,
                };
            }).ToList();
            // Đợi tất cả các tác vụ hoàn tất và trả về kết quả
            List<ProductResponse> productResponse = (await Task.WhenAll(productResponsesTasks)).ToList();
            if (isDescendPrice)
            {
                productResponse = productResponse.OrderByDescending(c => c.ProductPrice).ToList();
            }
            else
            {
                productResponse = productResponse.OrderBy(c => c.ProductPrice).ToList();
            }
            ModelDataPageResponse<List<ProductResponse>> result =
               Helpers.PaginationHelper.createPageDataResponse<List<ProductResponse>>(productResponse.Count, PageNumber, PageSize, false);
            productResponse = isPaging ? productResponse.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : productResponse;
            result.items = productResponse;
            return result;
        }

    }
}

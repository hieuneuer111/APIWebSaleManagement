using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Categories
{
    public class CategoryService : ICategoryServices
    {
        private readonly Supabase.Client _supabaseClient;
        public CategoryService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }
        public async Task<CategoryResponse> GetCategoryByIdAsync(Guid id)
        {
            CategoryResponse categoriesResponse = new CategoryResponse();
            // Gọi từ bảng CategoriesItemsModel và sử dụng phương thức Where để lọc theo categoryID
            ModeledResponse<CategoriesModel> supabaseResponseCategory = await _supabaseClient
                .From<CategoriesModel>()
                .Where(c => c.Category_Id == id)  // Sử dụng điều kiện để lọc theo categoryID
                .Get();
            // Kiểm tra xem có dữ liệu không
            if (supabaseResponseCategory.Models != null && supabaseResponseCategory.Models.Any())
            {
                // Chuyển kết quả từ SupabaseResponse thành List và gán cho response
                CategoriesModel CategoriesItems = supabaseResponseCategory.Models.FirstOrDefault();
                CategoryResponse resultCategoriesItemResponse = new CategoryResponse
                {
                    CategoryId = CategoriesItems.Category_Id,
                    CategoryName = CategoriesItems.Category_Name,
                    CategoryDes = CategoriesItems.Category_Description,
                };
                // Gán dữ liệu vào categoriesResponse (Giả sử CategoriesResponse có một danh sách CategoriesItems)
                categoriesResponse = resultCategoriesItemResponse;
            }
            // Trả về response
            return categoriesResponse;
        }

        public async Task<ModelResponse> AddCategoryItemsAsync(CategoryResquest category)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<CategoriesModel> addResponse = await _supabaseClient.From<CategoriesModel>().Insert(new CategoriesModel
            {
                Category_Description = category.RCategoryDes,
                Category_Name = category.RCategoryName, 
            });
            if (addResponse == null || addResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Add Errors");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Add Success!");
            }
            return response;
        }

        public async Task<ModelResponse> DeleteCategoryItemsAsync(Guid categoryId)
        {
            ModelResponse response = new ModelResponse();
            CategoriesModel modelToDelete = new CategoriesModel { Category_Id = categoryId }; // Tạo mô hình muốn xóa
            ModeledResponse<CategoriesModel> deleteResponse = await _supabaseClient
                .From<CategoriesModel>()
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
                response.ValidationMessages.Add("Delete Success!");
            }
            return response;
        }



        public async Task<ModelDataPageResponse<List<CategoryResponse>>> GetCategoryItemsAsync(string search, int PageNumber, int PageSize, bool isPaging)
        {
            ModeledResponse<CategoriesModel> SupabaseResponseCategory = await _supabaseClient.From<CategoriesModel>().Get();
            List<CategoriesModel> ListcategoriesItems = SupabaseResponseCategory.Models.ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                StringConvert conv = new StringConvert();
                search = conv.ConvertToUnSign(search);
                ListcategoriesItems = ListcategoriesItems.Where(tb => conv.ConvertToUnSign(tb.Category_Name).Contains(search)).ToList();
            }
            List<CategoryResponse> resultCategoriesItemResponse = ListcategoriesItems.Select(u => new CategoryResponse
            {
                CategoryId = u.Category_Id,
                CategoryName = u.Category_Name,
                CategoryDes = u.Category_Description,
            }).ToList();
            ModelDataPageResponse<List<CategoryResponse>> result =
                Helpers.PaginationHelper.createPageDataResponse<List<CategoryResponse>>(resultCategoriesItemResponse.Count, PageNumber, PageSize, false);
            resultCategoriesItemResponse = isPaging ? resultCategoriesItemResponse.Skip((result.currentPage - 1) * result.pageSize).Take(result.pageSize).ToList() : resultCategoriesItemResponse;
            result.items = resultCategoriesItemResponse;
            return result;
        }

        public async Task<ModelResponse> UpdateCategoryItemsAsync(CategoryResponse category)
        {
            ModelResponse response = new ModelResponse();
            ModeledResponse<CategoriesModel> updateResponse = await _supabaseClient
                              .From<CategoriesModel>()
                              .Where(x => x.Category_Id == category.CategoryId)
                              .Set(x => x.Category_Name, category.CategoryName)
                              .Set(x => x.Category_Description, category.CategoryDes)
                              .Update();
            if (updateResponse == null || updateResponse.Models.Count <= 0)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Update Errors. Category is empty");
            }
            else
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Update Success!");
            }
            return response;
        }
    }
}

using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Categories
{
    public interface ICategoryServices
    {
        Task<CategoryResponse> GetCategoryByIdAsync(Guid id);
        Task<ModelDataPageResponse<List<CategoryResponse>>> GetCategoryItemsAsync(string search, int PageNumber, int PageSize, bool isPaging);
        Task<ModelResponse> UpdateCategoryItemsAsync(CategoryResponse category);
        Task<ModelResponse> AddCategoryItemsAsync(CategoryResquest category);
        Task<ModelResponse> DeleteCategoryItemsAsync(Guid categoryId);
    }
}

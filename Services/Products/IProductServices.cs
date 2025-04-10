using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Products
{
    public interface IProductServices
    {
        Task<ProductResponse> GetProductById(Guid productId);
        Task<ModelResponse> UpdateImgUrlProduct(string productId, string urlProduct);
        Task<ModelDataResponse<ProductResponse>> AddProductAsync(ProductResquest productResquest);
        Task<ModelResponse> DeleteProductItemsAsync(Guid productId);
        Task<ModelDataPageResponse<List<ProductResponse>>> GetProductAsync(string search, List<string> category, int PageNumber, int PageSize, bool isPaging, bool isDescendPrice);
    }
}

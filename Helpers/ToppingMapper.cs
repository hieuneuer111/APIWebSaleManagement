using WebAPISalesManagement.ModelRequests;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Models;

namespace WebAPISalesManagement.Helpers
{
    public static class ToppingMapper
    {
        public static ToppingResponse ToResponse(this ToppingModel model)
        {
            return new ToppingResponse
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                IsActive = model.IsActive,
                CreatedAt = model.CreatedAt
            };
        }

        public static ToppingModel ToModel(this ToppingRequest request)
        {
            return new ToppingModel
            {
                Name = request.Name,
                Price = request.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}

using Supabase.Postgrest.Responses;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelRequests;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Models;
using WebAPISalesManagement.Services.Toppings;
using WebAPISalesManagement.Services.SupabaseClient;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services
{
    public class ToppingService : IToppingService
    {
        private readonly Supabase.Client _client;

        public ToppingService(Supabase.Client client)
        {
            _client = client;
        }

        public async Task<ModelDataResponse<ToppingResponse>> AddToppingAsync(ToppingRequest request)
        {
            ModelDataResponse<ToppingResponse> response = new();

            var newTopping = new ToppingModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _client.From<ToppingModel>().Insert(newTopping);
            if (result.Models.Count > 0)
            {
                var added = result.Models.First();
                response.ItemResponse = new ToppingResponse
                {
                    Id = added.Id,
                    Name = added.Name,
                    Price = added.Price,
                    IsActive = added.IsActive,
                    CreatedAt = added.CreatedAt
                };
                response.IsValid = true;
                response.ValidationMessages.Add("Thêm topping thành công!");
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Thêm topping thất bại.");
            }

            return response;
        }

        public async Task<ModelResponse> UpdateToppingAsync(Guid id, ToppingRequest request)
        {
            ModelResponse response = new();

            var result = await _client
                .From<ToppingModel>()
                .Where(x => x.Id == id)
                .Get();

            var topping = result.Models.FirstOrDefault();
            if (topping == null)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Topping không tồn tại.");
                return response;
            }

            topping.Name = request.Name;
            topping.Price = request.Price;

            var updateResult = await _client.From<ToppingModel>().Update(topping);
            if (updateResult.Models.Count > 0)
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Cập nhật topping thành công.");
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Cập nhật thất bại.");
            }

            return response;
        }

        public async Task<ModelResponse> DeleteToppingAsync(Guid id)
        {
            ModelResponse response = new();

            var result = await _client
                .From<ToppingModel>()
                .Where(x => x.Id == id)
                .Get();

            var topping = result.Models.FirstOrDefault();
            if (topping == null)
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Topping không tồn tại.");
                return response;
            }

            topping.IsActive = false;

            var deleteResult = await _client.From<ToppingModel>().Update(topping);
            if (deleteResult.Models.Count > 0)
            {
                response.IsValid = true;
                response.ValidationMessages.Add("Xoá topping thành công (ẩn topping).");
            }
            else
            {
                response.IsValid = false;
                response.ValidationMessages.Add("Không thể xoá topping.");
            }

            return response;
        }

        public async Task<ToppingResponse?> GetToppingByIdAsync(Guid id)
        {
            var result = await _client
                .From<ToppingModel>()
                .Where(x => x.Id == id)
                .Get();

            var topping = result.Models.FirstOrDefault();
            return topping == null ? null : new ToppingResponse
            {
                Id = topping.Id,
                Name = topping.Name,
                Price = topping.Price,
                IsActive = topping.IsActive,
                CreatedAt = topping.CreatedAt
            };
        }

        public async Task<ModelDataResponse<List<ToppingResponse>>> GetAllActiveToppingsAsync()
        {
            ModelDataResponse<List<ToppingResponse>> response = new ModelDataResponse<List<ToppingResponse>>();
            try
            {
                var result = await _client
                 .From<ToppingModel>()
                 .Where(x => x.IsActive == true)
                 .Get();
                List<ToppingResponse> resultItemResponse = result.Models
                   .Select(x => new ToppingResponse
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Price = x.Price,
                       IsActive = x.IsActive,
                       CreatedAt = x.CreatedAt
                   })
                   .ToList();
                response.IsValid = true;
                response.ItemResponse = resultItemResponse;
            }
            catch (Exception ex)
            {
                response.IsValid = false;
                response.ValidationMessages.Add(ex.Message);
            }
            return response;                   
        }

        public async Task<ModelDataResponse<List<ToppingResponse>>> GetAllToppingsAsync()
        {
            ModelDataResponse<List<ToppingResponse>> response = new ModelDataResponse<List<ToppingResponse>>();
            try
            {
               
                var result = await _client
                    .From<ToppingModel>()
                    .Get();
                List<ToppingResponse> resultItemResponse = result.Models
                   .Select(x => new ToppingResponse
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Price = x.Price,
                       IsActive = x.IsActive,
                       CreatedAt = x.CreatedAt
                   })
                   .ToList();
                response.IsValid = true;
                response.ItemResponse = resultItemResponse;
            }
            catch (Exception ex) { 
                response.IsValid = false;
                response.ValidationMessages.Add(ex.Message);
            }           
            return response;
        }
    }
}

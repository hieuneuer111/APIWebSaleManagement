using WebAPISalesManagement.ModelRequests;
using WebAPISalesManagement.ModelResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Services.Toppings
{
    public interface IToppingService
    {
        /// <summary>
        /// Lấy tất cả topping đang active
        /// </summary>
        Task<ModelDataResponse<List<ToppingResponse>>> GetAllActiveToppingsAsync();

        /// <summary>
        /// Lấy tất cả topping (kể cả đã bị ẩn)
        /// </summary>
        Task<ModelDataResponse<List<ToppingResponse>>> GetAllToppingsAsync();

        /// <summary>
        /// Lấy topping theo ID
        /// </summary>
        Task<ToppingResponse?> GetToppingByIdAsync(Guid id);

        /// <summary>
        /// Thêm topping mới
        /// </summary>
        Task<ModelDataResponse<ToppingResponse>> AddToppingAsync(ToppingRequest request);

        /// <summary>
        /// Cập nhật topping
        /// </summary>
        Task<ModelResponse> UpdateToppingAsync(Guid id, ToppingRequest request);

        /// <summary>
        /// Xoá mềm topping
        /// </summary>
        Task<ModelResponse> DeleteToppingAsync(Guid id);
    }
}

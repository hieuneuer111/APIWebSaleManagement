using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Services.Categories;
using WebAPISalesManagement.Services.Products;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Controllers
{
    //[Authorize] // Đảm bảo rằng người dùng đã đăng nhập
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeRight("manage_products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        public ProductController(IProductServices productServices, ICategoryServices categoryServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
        }
        /// <summary>
        /// Get All Product Return List
        /// </summary>
        /// <param name="search"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="isPaging"></param>
        /// <param name="isDecPrice"></param>
        /// <param name="categoryItem"></param>
        /// <returns></returns>
        [HttpPost("GetItemsMenu")]
        public async Task<ActionResult> GetProduct(string search = "", int PageNumber = 1, int PageSize = 10, bool isPaging = false, bool isDecPrice = true, List<string> categoryItem = null)
        {
            try
            {
                ModelDataPageResponse<List<ProductResponse>> result = await _productServices.GetProductAsync(search = "", categoryItem, PageNumber, PageSize, isPaging, isDecPrice = true);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lấy product dựa vào product id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [HttpGet("GetProductById")]
        public async Task<ActionResult> GetProductById(string requestId)
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ProductResponse result = await _productServices.GetProductById(Guid.Parse(requestId));
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        public async Task<ActionResult> AddProduct([FromBody] ProductResquest product)
        {
            try
            {
                ModelDataResponse<ProductResponse> result = await _productServices.AddProductAsync(product);
                return result.IsValid ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult> DeleteProduct( string productId)
        {
            try
            {
                ModelResponse result = await _productServices.DeleteProductItemsAsync(Guid.Parse(productId));
                return result.IsValid ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update Infomation Product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productResquest"></param>
        /// <returns></returns>
        [HttpPost("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(string productId,[FromBody] ProductResquest productResquest)
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ModelResponse result = await _productServices.UpdateProduct(Guid.Parse(productId), productResquest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Thay đổi địa chỉ hình ảnh sản phẩm
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="urlImg"></param>
        /// <returns></returns>
        [HttpPost("UpdateProductImgURL")]
        public async Task<ActionResult> UpdateProductImgURL(string productId, string urlImg)
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ModelResponse result = await _productServices.UpdateImgUrlProduct(productId, urlImg);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Láy ds danh mục sản phẩm
        /// </summary>
        /// <param name="search"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="isPaging"></param>
        /// <returns></returns>
        
        [HttpGet("GetCategories")]
        public async Task<ActionResult> GetCategoriesAsync(string search = "", int PageNumber = 1, int PageSize = 10, bool isPaging = false)
        {
            try
            {
                ModelDataPageResponse<List<CategoryResponse>> result = await _categoryServices.GetCategoryItemsAsync(search, PageNumber, PageSize, isPaging);
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        
        }
        /// <summary>
        /// Chỉnh sửa dm sản phẩm
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdateCategory")]
        public async Task<ActionResult> UpdateCategoryItems(CategoryResponse request)
        {
            try
            {
                ModelResponse result = await _categoryServices.UpdateCategoryItemsAsync(request);
                return result.IsValid ? Ok(result) : BadRequest(result);
            } 
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
          
        }
        /// <summary>
        /// Thêm dm sản phẩm
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddCategoryItems([FromBody] CategoryResquest category)
        {
            try
            {
                ModelResponse result = await _categoryServices.AddCategoryItemsAsync(category);
                return result.IsValid ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        
        }
        /// <summary>
        /// Xóa dm sản phẩm
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult> DeleteCategoryItems(string categoryId)
        {
            try
            {
                ModelResponse result = await _categoryServices.DeleteCategoryItemsAsync(Guid.Parse(categoryId));
                return result.IsValid ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        /// <summary>
        /// Lấy chi tiết dm sản phẩm theo id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("GetDetailCategory")]
        public async Task<ActionResult> GetDetailCategory(string categoryId)
        {
            try
            {
                CategoryResponse result = await _categoryServices.GetCategoryByIdAsync(Guid.Parse(categoryId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

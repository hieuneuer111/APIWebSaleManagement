using Microsoft.AspNetCore.Mvc;
using WebAPISalesManagement.ModelRequests;
using WebAPISalesManagement.Services.Toppings;

namespace WebAPISalesManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToppingController : ControllerBase
    {
        private readonly IToppingService _toppingService;

        public ToppingController(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        /// <summary>
        /// Lấy danh sách tất cả topping đang hoạt động
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetAllActive()
        {
            var data = await _toppingService.GetAllActiveToppingsAsync();
            return Ok(data);
        }

        /// <summary>
        /// Lấy tất cả topping (bao gồm đã bị ẩn)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _toppingService.GetAllToppingsAsync();
            return Ok(data);
        }

        /// <summary>
        /// Lấy chi tiết topping theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return BadRequest(new { message = "ID không hợp lệ." });

            var topping = await _toppingService.GetToppingByIdAsync(guid);
            if (topping == null)
                return NotFound(new { message = "Topping không tồn tại." });

            return Ok(topping);
        }

        /// <summary>
        /// Thêm mới topping
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToppingRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            var result = await _toppingService.AddToppingAsync(request);
            if (!result.IsValid)
                return StatusCode(500, result);

            return CreatedAtAction(nameof(GetById), new { id = result.ItemResponse.Id }, result);
        }

        /// <summary>
        /// Cập nhật topping
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ToppingRequest request)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return BadRequest(new { message = "ID không hợp lệ." });

            var result = await _toppingService.UpdateToppingAsync(guid, request);
            if (!result.IsValid)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Xóa mềm topping (ẩn topping khỏi danh sách)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return BadRequest(new { message = "ID không hợp lệ." });

            var result = await _toppingService.DeleteToppingAsync(guid);
            if (!result.IsValid)
                return NotFound(result);

            return Ok(result);
        }
    }
}

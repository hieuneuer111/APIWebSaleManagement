using Microsoft.AspNetCore.Mvc;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Services.Categories;
using WebAPISalesManagement.Services.Discounts;
using WebAPISalesManagement.Services.Products;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountServices _discountServices;
        public DiscountController(IDiscountServices discountServices)
        {
           _discountServices = discountServices;
        }
        /// <summary>
        /// TypeDiscount Nếu = 1 thì lọc tất cả Giảm giá theo % "Percentage", = 2 thì theo giá tiền "FixedAmount", valid == 1 mã còn hạn, 2 hết hạn, null lấy all
        /// </summary>
        /// <param name="search"></param>
        /// <param name="typeDiscount"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="isPaging"></param>
        /// <returns></returns>
        [HttpPost("GetDiscountList")]
        public async Task<ActionResult> GetDiscountList(string search = "", int typeDiscount = 0, int PageNumber = 1, int PageSize = 10, bool isPaging = false, DateTime? dateStart = null, DateTime? dateEnd = null, int valid = 0)
        {
            try
            {
                ModelDataPageResponse<List<DiscountResponse>> result = await _discountServices.GetDiscountListAsync(search = "", typeDiscount, PageNumber, PageSize, isPaging,dateStart,dateEnd, valid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lấy chi tiết discount
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns></returns>
        [HttpGet("GetDiscountItem")]
        public async Task<ActionResult> GetDiscountItem(string discountId = "")
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                DiscountResponse result = await _discountServices.GetDiscountItemsAsync(Guid.Parse(discountId));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gia hạn mã giảm giá
        /// </summary>
        /// <param name="newDateEnd"></param>
        /// <param name="discountId"></param>
        /// <returns></returns>
        [HttpPost("ExtendDiscount")]
        public async Task<ActionResult> ExtendDiscountAsync(DateTime newDateEnd,string discountId = "")
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ModelResponse result = await _discountServices.ExtendDiscountAsync(Guid.Parse(discountId), newDateEnd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Hủy hoặc kích hoạt mã giảm giá
        /// </summary>
        /// <param name="active"></param>
        /// <param name="discountId"></param>
        /// <returns></returns>
        [HttpPost("ActiveDiscount")]
        public async Task<ActionResult> ActiveDiscountAsync(bool active, string discountId = "")
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ModelResponse result = await _discountServices.ActiveDiscountAsync(Guid.Parse(discountId), active);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


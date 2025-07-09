using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPISalesManagement.Helpers;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.Services.Categories;
using WebAPISalesManagement.Services.Products;
using WebAPISalesManagement.Services.Reports;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [AuthorizeRight("view_reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportServices _reportServices;
        public ReportController(IReportServices reportServices)
        {
            _reportServices = reportServices;
        }
        /// <summary>
        /// Lấy ds sản phâm bán chạy trong khoảng thời gian
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateEnd"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpPost("GetProductBestSaler")]
        public async Task<ActionResult> GetProductBestSaler(DateTime? dateFrom = null, DateTime? dateEnd = null,int top =10)
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ModelDataResponse<List<SP_BestSellingProductResponse>> result = await _reportServices.BestSellingProduct(dateFrom,dateEnd,top);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Thống kê doanh thu theo thời gian Tổng tiền gốc (total_amount), Tổng giảm giá (discount_value), Tổng tiền thực thu (final_total)
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [HttpPost("GetRevenueReportAsync")]
        public async Task<ActionResult> GetRevenueReportAsync(DateTime? dateFrom = null, DateTime? dateEnd = null)
        {
            try
            {
                //Gu id = Guid.Parse(requestId);
                ModelDataResponse<RevenueReportResponse> result = await _reportServices.GetRevenueReportAsync(dateFrom, dateEnd);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

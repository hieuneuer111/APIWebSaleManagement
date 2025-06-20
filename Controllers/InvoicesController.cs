using Microsoft.AspNetCore.Mvc;
using WebAPISalesManagement.ModelResponses;
using WebAPISalesManagement.ModelResquests;
using WebAPISalesManagement.Services.Categories;
using WebAPISalesManagement.Services.Invoices;
using WebAPISalesManagement.Services.Products;
using WebAPISalesManagement.Swagger;

namespace WebAPISalesManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        //[Authorize] // Đảm bảo rằng người dùng đã đăng nhập
       
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IInvoiceService  _invoiceService;
        public InvoicesController(IProductServices productServices, ICategoryServices categoryServices, IInvoiceService invoiceService)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
            _invoiceService = invoiceService;
        }
        /// <summary>
        /// Get Product List By Invoice
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        [HttpGet("GetProductByInvoice")]
        public async Task<ActionResult> GetProductByInvoice(string idInvoice)
        {
            try
            {
                ModelDataResponse<List<SP_GetProductByInvoiceResponce>> result = await _invoiceService.GetProductByInvoiceId(Guid.Parse(idInvoice));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lấy chi tiết hóa đơn dựa vào id
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        [HttpGet("GetInvoiceById")]
        public async Task<ActionResult> GetInvoiceById(string idInvoice)
        {
            try
            {
                ModelDataResponse<InvoiceDetailResponse> result = await _invoiceService.GetInvoiceById(Guid.Parse(idInvoice));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Lấy ds Hóa đơn 
        /// Mặc định dateStart và dateEnd null lọc all
        /// nếu dateStart tồn tại thì lọc từ dateStart tới Nay
        /// Nếu dateEnd tồn tại thì lọc từ trước tới dateEnd
        /// </summary>
        /// <param name="search"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="isPaging"></param>
        /// <param name="isDecPrice"></param>
        /// <param name="userCreater"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        [HttpPost("GetInvoiceList")]
        public async Task<ActionResult> GetInvoiceList(string search = "", int PageNumber = 1, int PageSize = 10, bool isPaging = false, bool isDecPrice = true, List<string> userCreater = null, DateTime? dateStart = null, DateTime? dateEnd = null,int status = 0)
        {
            try
            {
                ModelDataPageResponse<List<InvoiceListResponse>> result = await _invoiceService.GetInvoiceList(dateStart,dateEnd, userCreater,PageNumber,PageSize,isPaging,isDecPrice,status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Hàm tạo hóa đơn có rollback bằng store nếu lỗi
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateInvoice")]
        public async Task<ActionResult> CreateInvoice(InvoiceRequest request)
        {
            try
            {
                ModelDataResponse<Guid?> result = await _invoiceService.CreateInvoiceAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Thanh toán hóa đơn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("PaymentInvoice")]
        public async Task<ActionResult> PaymentInvoice(InvoicePaymentRequest request)
        {
            try
            {
                ModelResponse result = await _invoiceService.PayInvoiceAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

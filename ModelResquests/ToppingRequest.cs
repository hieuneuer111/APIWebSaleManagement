using System.ComponentModel.DataAnnotations;

namespace WebAPISalesManagement.ModelRequests
{
    public class ToppingRequest
    {
        [Required(ErrorMessage = "Tên topping không được để trống")]
        public string Name { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Giá topping phải >= 0")]
        public int Price { get; set; }
    }
}

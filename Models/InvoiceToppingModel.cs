using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace WebAPISalesManagement.Models
{
    [Table("invoicetoppings")]
    public class InvoiceToppingModel : BaseModel
    {
        [PrimaryKey("invoice_id", false)]
        public Guid InvoiceId { get; set; }

        [PrimaryKey("product_id", false)]
        public Guid ProductId { get; set; }

        [PrimaryKey("topping_id", false)]
        public Guid ToppingId { get; set; }

        [Column("topping_price")]
        public int ToppingPrice { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 1;
    }
}

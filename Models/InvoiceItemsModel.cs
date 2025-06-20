using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPISalesManagement.Models
{
    [Table("invoiceitems")]
    public class InvoiceItemsModel : BaseModel
    {
        [PrimaryKey("invoice_id", false)]
        [Column("invoice_id")]
        public Guid Invoice_Id { get; set; }

        [PrimaryKey("product_id", false)]
        [Column("product_id")]
        public Guid Product_Id { get; set; }

        [Column("unit_price")]
        public long Unit_Price { get; set; }

        [Column("line_total")]
        public int Line_Total { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("discount_id")]
        public Guid? Discount_Id { get; set; }

        [Column("total_price")]
        public long Total_Price { get; set; }

        [Column("discount_snapshot_name")]
        public string? Discount_Snapshot_Name { get; set; }

        [Column("discount_type")]
        public string? Discount_Type { get; set; }

        [Column("discount_raw_value")]
        public string? Discount_Raw_Value { get; set; }

        [Column("discount_value")]
        public long? Discount_Value { get; set; }
    }
}

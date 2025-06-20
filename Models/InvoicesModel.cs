using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebAPISalesManagement.Models
{
    [Table("invoices")]
    public class InvoicesModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Invoice_Id { get; set; }

        [Column("user_id")]
        public Guid Invoice_Creater { get; set; }

        [Column("total_amount")]
        public long Total_Amount { get; set; }

        [Column("status")]
        public bool Invoice_Status { get; set; }

        [Column("created_at")]
        public DateTime Created_at { get; set; }

        [Column("discount_value")]
        public long Discount_Value { get; set; }

        [Column("final_total")]
        public long Final_Total { get; set; }

        [Column("discount_id")]
        public Guid? Discount_Id { get; set; }

        [Column("discount_snapshot_name")]
        public string? Discount_Snapshot_Name { get; set; }

        [Column("discount_type")]
        public string? Discount_Type { get; set; }

        [Column("discount_raw_value")]
        public string? Discount_Raw_Value { get; set; }
        [Column("payment_method")]
        public string? Payment_Method { get; set; }

        [Column("payment_time")]
        public DateTime? Payment_Time { get; set; }
    }
}

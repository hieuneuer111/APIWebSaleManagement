using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace WebAPISalesManagement.Models
{
    [Table("discounts")]
    public class DiscountsModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("percentage")]
        public int? Percentage { get; set; }

        [Column("fixed_amount")]
        public long? FixedAmount { get; set; }

        [Column("min_invoice_total")]
        public long? MinInvoiceTotal { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("isactive")]
        public bool IsActive { get; set; }
    }
}

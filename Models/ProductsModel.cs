using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Reactive;
using System.Xml.Linq;
using System;

namespace WebAPISalesManagement.Models
{
    [Table("products")]
    public class ProductsModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Product_Id { get; set; }
        [Column("name")]
        public string Product_Name { get; set; }
        [Column("price")]
        public long Product_Price { get; set; }
        [Column("category_id")]
        public Guid Product_Category { get; set; }
        [Column("status")]
        public bool Product_Status { get; set; }
        [Column("description")]
        public string Product_Des { get; set; }
        [Column("image_url")]
        public string Product_ImgURL { get; set; }
        [Column("created_at")]
        public DateTime created_at { get; set; }
        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}

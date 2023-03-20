#nullable disable

using AppCore.Records.Bases;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Business.Models
{
    public class ProductModel : RecordBase
    {
        #region Entity'den kopyalanan özellikler
        //[Required(ErrorMessage = "Product Name is required!")]
        [Required(ErrorMessage = "{0} is required!")]
        //[StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [MinLength(2, ErrorMessage = "{0} must be minimum {1} characters!")] // Product Name must be minimum 2 characters!
        [MaxLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Description { get; set; }

        [DisplayName("Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} must be 0 or bigeer number!")]
        [Required(ErrorMessage = "{0} is required!")]
        public double? UnitPrice { get; set; }

        [Range(0, 1000000, ErrorMessage = "{0} must be minimum {1} and {2}!")]
        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Stock Amount")]
        public int? StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? CategoryId { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }

        [StringLength(5, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string ImageExtension { get; set; }
        #endregion

        #region Entity Dışı Özelleştirmeler
        [DisplayName("Unit Price")]
        public string UnitPriceDisplay { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateDisplay { get; set; }

        [DisplayName("Category")]
        public string CategoryNameDisplay { get; set; }

        [DisplayName("Stores")]
        public List<int> StoreIds { get; set; }

        [DisplayName("Stores")]
        public string StoreNamesDisplay { get; set; }

        [DisplayName("Image")]
        public string ImgSrcDisplay { get; set; }
        #endregion
    }
}

#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class CategoryModel : RecordBase
    {
        #region Entity'den kopyalanan özellikler
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        #region Sayfada ihtiyacımız olan özellikler
        [DisplayName("Product Count")]
        public int? ProductCountDisplay { get; set; }
        #endregion
    }
}

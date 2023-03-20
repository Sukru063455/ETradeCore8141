#nullable disable

using DataAccess.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserDetailModel
    {
        public Sex Sex { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(250, ErrorMessage = "{0} must be maximum {1} characters!")]
        [EmailAddress(ErrorMessage = "{0} must be in e-mail format!")]
        [DisplayName("E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(1000, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("City")]
        public int? CityId { get; set; }
    }
}

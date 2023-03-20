#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel : RecordBase
    {
        #region Entity
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
        #endregion

        #region Yeni İhtiyaç
        public string RoleName { get; set; }

        public UserDetailModel UserDetail { get; set; }
        #endregion
    }
}

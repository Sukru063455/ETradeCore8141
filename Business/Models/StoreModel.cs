#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class StoreModel : RecordBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Virtual")]
        public bool IsVirtual { get; set; }

        public bool IsDeleted { get; set; }

        [DisplayName("Virtual")]
        public string IsVirtualDisplay { get; set; }
    }
}

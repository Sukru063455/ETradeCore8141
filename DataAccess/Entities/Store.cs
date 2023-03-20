#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Store : RecordBase, ISoftDelete
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsVirtual { get; set; }

        public bool IsDeleted { get; set; }

        public List<ProductStore> ProductStores { get; set; }
    }
}

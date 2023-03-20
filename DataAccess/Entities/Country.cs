#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    //[Table("Ulkeler")]
    public class Country : RecordBase
    {
        [Required]
        [StringLength(100)]
        //[Column("Adi", TypeName = "varchar(100)")]
        public string Name { get; set; }

        public List<City> Cities { get; set; }

        public List<UserDetail> UserDetails { get; set; }
    }
}

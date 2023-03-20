#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class CartItemModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Unit Price")]
        public double UnitPrice { get; set; }
    }
}

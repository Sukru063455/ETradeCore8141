#nullable disable

using System.ComponentModel;

namespace Business.Models
{
    public class CartItemGroupByModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        public string TotalUnitPrice { get; set; }

        public string TotalCount { get; set; }



        public double TotalUnitPriceValue { get; set; }
        public int TotalCountValue { get; set; }
    }
}

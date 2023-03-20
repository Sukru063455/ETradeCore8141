#nullable disable

using System.ComponentModel;

namespace Business.Models.Report
{
    public class ReportItemModel
    {
        #region Report
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPrice { get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmount { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDate { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        [DisplayName("Store")]
        public string StoreName { get; set; }
        #endregion

        #region Filter
        public int? CategoryId { get; set; }

        public DateTime? ExpirationDateValue { get; set; }

        public int? StoreId { get; set; }
        #endregion
    }
}

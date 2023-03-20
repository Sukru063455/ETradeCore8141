#nullable disable

using System.ComponentModel;

namespace Business.Models.Report
{
    public class ReportFilterModel
    {
        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Product")]
        public string ProductName { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDateBegin { get; set; }

        public DateTime? ExpirationDateEnd { get; set; }

        [DisplayName("Stores")]
        public List<int> StoreIds { get; set; }
    }
}

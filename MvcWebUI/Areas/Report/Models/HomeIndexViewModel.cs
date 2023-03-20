#nullable disable

using Business.Models.Report;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcWebUI.Areas.Report.Models
{
    public class HomeIndexViewModel
    {
        public List<ReportItemModel> Report { get; set; }
        public SelectList Categories { get; set; }
        public MultiSelectList Stores { get; set; }
        public ReportFilterModel Filter { get; set; }
    }
}

using Business.Models.Report;
using Business.Services;
using Business.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Areas.Report.Models;

namespace MvcWebUI.Areas.Report.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Report")]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

        public HomeController(IReportService reportService, ICategoryService categoryService, IStoreService storeService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _storeService = storeService;
        }

        // GET: HomeController
        public IActionResult Index(HomeIndexViewModel viewModel)
        {
            viewModel.Report = _reportService.GetReport(viewModel.Filter);
            viewModel.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            viewModel.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
            return View(viewModel);
        }
    }
}

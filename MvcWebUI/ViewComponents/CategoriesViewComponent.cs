using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace MvcWebUI.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke()
        {
            List<CategoryModel> categories;
            Task<List<CategoryModel>> task;

            //categories = _categoryService.Query().ToList(); // synchronous programming

            //task = _categoryService.Query().ToListAsync(); // asynchronous programming
            //categories = task.Result;

            task = _categoryService.GetListAsync();
            categories = task.Result;

            return View(categories);
        }
    }
}

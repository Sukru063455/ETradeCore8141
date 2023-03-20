using Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/Kategoriler")] // ~/api/Kategoriler
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet("Listele")] // ~/Kategoriler/Listele
		public IActionResult Listele()
		{
			return Ok(_categoryService.Query().ToList());
		}
	}
}

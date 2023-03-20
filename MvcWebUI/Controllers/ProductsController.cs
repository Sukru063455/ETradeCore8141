using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// DB First yaklaşım için Package Manager Console'da çalıştırılmalı:
// Scaffold-DbContext "server=.\SQLEXPRESS;database=ETradeCore8141;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities

namespace MvcWebUI.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

		public ProductsController(IProductService productService, ICategoryService categoryService, IStoreService storeService)
		{
			_productService = productService;
			_categoryService = categoryService;
			_storeService = storeService;
		}

        [AllowAnonymous]
		public IActionResult Index()
        {
            var products = _productService.Query().ToList();
            return View(products);
        }

        //[HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //ViewData["Categories"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create(string Name, string Description, double UnitPrice, int StockAmount, DateTime? ExpirationDate, int CategoryId)
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductModel product, IFormFile? uploadedImage)
        {
            if (ModelState.IsValid)
            {
                Result result;

                result = UpdateImage(product, uploadedImage);

                if (result.IsSuccessful)
                {
                    result = _productService.Add(product);
                    if (result.IsSuccessful)
                    {
                        //return RedirectToAction("Index", "Products");
                        //return RedirectToAction("Index");
                        TempData["Message"] = result.Message; // success
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["Message"] = result.Message; // error
            }
            ViewBag.Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
			return View(product);
        }

        // Products/Edit/1
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id) // controller/action/id?
        {
            var product = _productService.Query().SingleOrDefault(p => p.Id == id);
            if (product is null)
            {
                //return NotFound();
                return View("_Error", "Product not found!");
            }
            ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
			return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductModel product, IFormFile? uploadedImage)
        {
            if (ModelState.IsValid)
            {
                Result result = UpdateImage(product, uploadedImage);
                
                if (result.IsSuccessful)
                {
                    result = _productService.Update(product);
                    if (result.IsSuccessful)
                    {
                        TempData["Message"] = result.Message; // success
                        return RedirectToAction(nameof(Index));
                    } 
                }
                ModelState.AddModelError("", result.Message); // error
            }
			ViewBag.CategoryId = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
			ViewBag.Stores = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name", product.StoreIds);
			return View(product);
		}

        public IActionResult Delete(int id)
        {
            //if (!(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Login", "Users", new { area = "Account" });
            var result = _productService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var product = _productService.Query().SingleOrDefault(p => p.Id == id);
            if (product is null)
                return View("_Error", "Product not found!");
            return View(product);
        }

        public IActionResult DeleteImage(int id)
        {
            var result = _productService.DeleteImage(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }

        private Result UpdateImage(ProductModel resultModel, IFormFile uploadedImage)
        {
            Result result = new SuccessResult();

            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                string uploadedFileName = uploadedImage.FileName; // asusrog.jpg
                string uploadedFileExtension = Path.GetExtension(uploadedFileName); // .jpg

                if (!AppSettings.AcceptedExtensions.Split(',').Any(ae => ae.ToLower().Trim() == uploadedFileExtension.ToLower()))
                    result = new ErrorResult($"Image can't be uploaded because accepted extensions are {AppSettings.AcceptedExtensions}!");

                if (result.IsSuccessful)
                {
                    double acceptedFileLength = AppSettings.AcceptedLength;
                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 byte
                    // 1 mega byte = 1024 kilobyte = 1024 * 1024 byte = 1.048.576 byte
                    double acceptedFileLengthInBytes = acceptedFileLength * Math.Pow(1024, 2);

                    if (uploadedImage.Length > acceptedFileLengthInBytes)
                        result = new ErrorResult("Image can't be uploaded because accepted file size is " + AppSettings.AcceptedLength.ToString("N1") + "!");
                }

                if (result.IsSuccessful)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        uploadedImage.CopyTo(memoryStream);
                        resultModel.Image = memoryStream.ToArray();
                        resultModel.ImageExtension = uploadedFileExtension;
                    }
                }

            }
            return result;
        }

        [Obsolete("Bu method eski versiyondur, yeni olan DeleteImage'ı kullan!")]
        public IActionResult DeleteImageEski(int id)
        {
            var result = _productService.DeleteImage(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}

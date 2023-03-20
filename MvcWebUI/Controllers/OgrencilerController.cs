using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    public class OgrencilerController : Controller
    {
        public IActionResult Getir(string adi, string soyadi, int yasi)
        {
            return Content("Adı: " + adi + ", Soyadı: " + soyadi + ", Yaşı: " + yasi);
        }

        public IActionResult Get()
        {
            return View("GetForm");
        }

        public IActionResult Post(string adi, string soyadi, int yasi)
        {
            return Content("Adı: " + adi + ", Soyadı: " + soyadi + ", Yaşı: " + yasi);
        }
    }
}

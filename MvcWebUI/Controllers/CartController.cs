using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class CartController : Controller
    {
        private const string SESSIONKEY = "cart";

        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult GetCart()
        {
            List<CartItemModel> cart = GetCartFromSession();

            List<CartItemGroupByModel> cartGroupBy = (from c in cart
                                                      group c by new { c.ProductId, c.UserId, c.ProductName } into cGroupBy
                                                      select new CartItemGroupByModel()
                                                      {
                                                          ProductId = cGroupBy.Key.ProductId,
                                                          UserId = cGroupBy.Key.UserId,
                                                          ProductName = cGroupBy.Key.ProductName,
                                                          TotalUnitPrice = cGroupBy.Sum(cgb => cgb.UnitPrice).ToString("C2"),
                                                          TotalCount = cGroupBy.Count() + " pieces",
                                                          TotalUnitPriceValue = cGroupBy.Sum(cgb => cgb.UnitPrice),
                                                          TotalCountValue = cGroupBy.Count()
                                                      }).ToList();

            return View("CartGroupBy", cartGroupBy);
        }

        public IActionResult AddToCart(int productId)
        {
            List<CartItemModel> cart = GetCartFromSession();
            int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == productId);
            if (product.StockAmount == 0)
            {
                TempData["Message"] = "Product cannot be added to cart because there is no product in stock!";
                return RedirectToAction("Index", "Products");
            }
            CartItemModel cartItem = new CartItemModel()
            {
                ProductId = productId,
                ProductName = product.Name,
                UnitPrice = product.UnitPrice ?? 0,
                UserId = userId
            };
            cart.Add(cartItem);
            string cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SESSIONKEY, cartJson);
            TempData["Message"] = $"{product.Name} added to cart.";
            return RedirectToAction("Index", "Products");
        }

        private List<CartItemModel> GetCartFromSession()
        {
            List<CartItemModel> cart = new List<CartItemModel>();
            string cartJson = HttpContext.Session.GetString(SESSIONKEY);
            if (!string.IsNullOrWhiteSpace(cartJson))
            {
                cart = JsonConvert.DeserializeObject<List<CartItemModel>>(cartJson);
                
                int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                cart = cart.Where(c => c.UserId == userId).ToList();
            }
            return cart;
        }

        public IActionResult ClearCart()
        {
            //HttpContext.Session.Clear(); // tüm kullanıcı sessionları temizlenir
            //HttpContext.Session.Remove(SESSIONKEY); // bu key'e sahip session temizlenir

            List<CartItemModel> cart = new List<CartItemModel>();
            string cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SESSIONKEY, cartJson);

            TempData["Message"] = "Cart cleared.";
            return RedirectToAction(nameof(GetCart));
        }

        public IActionResult DeleteFromCart(int productId, int userId)
        {
            List<CartItemModel> cart = GetCartFromSession();
            CartItemModel cartItem = cart.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
            if (cartItem is not null)
                cart.Remove(cartItem);

            string cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(SESSIONKEY, cartJson);

            TempData["Message"] = "Item removed from cart.";
            return RedirectToAction(nameof(GetCart));
        }
    }
}

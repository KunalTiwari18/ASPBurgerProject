using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BBURGERClone.Helpers;
using BBURGERClone.Models;
using BBURGERClone.Services;

namespace BBURGERClone.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IProductService _productService;

        public CartController(ILogger<CartController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = new SessionCart(HttpContext.Session);
            var items = cart.GetItems();

            // populate Product for each item
            foreach (var it in items)
            {
                if (it.Product == null || it.Product.Id == 0)
                {
                    var p = _productService.GetById(it.ProductId);
                    if (p != null) it.Product = p;
                }
            }

            return View(items);
        }

        // POST: /Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId, int qty = 1)
        {
            var product = _productService.GetById(productId);
            if (product == null) return NotFound();

            var cart = new SessionCart(HttpContext.Session);
            cart.AddToCart(product, qty);

            // redirect back to home or cart (choose one)
            return RedirectToAction("Index", "Cart");
        }

        // POST: /Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            var cart = new SessionCart(HttpContext.Session);
            cart.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        // GET: /Cart/GetCount
        [HttpGet]
        public IActionResult GetCount()
        {
            try
            {
                var cart = new SessionCart(HttpContext.Session);
                var items = cart.GetItems();
                if (items == null || !items.Any()) return Json(new { count = 0 });
                int count = items.Sum(i => i.Quantity);
                return Json(new { count });
            }
            catch
            {
                return Json(new { count = 0 });
            }
        }
    }
}

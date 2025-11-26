using Microsoft.AspNetCore.Mvc;
using BBURGERClone.Services;

namespace BBURGERClone.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService ps)
        {
            _productService = ps;
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}

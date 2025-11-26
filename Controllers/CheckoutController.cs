using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BBURGERClone.Helpers;
using BBURGERClone.Models;
using BBURGERClone.Services; // IProductService

namespace BBURGERClone.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CheckoutController> _logger;
        private readonly IProductService _productService;

        public CheckoutController(IConfiguration config, ILogger<CheckoutController> logger, IProductService productService)
        {
            _config = config;
            _logger = logger;
            _productService = productService;
        }

        // GET: /Checkout/Payment
        public IActionResult Payment()
        {
            // Safe read of cart from session
            var cart = new SessionCart(HttpContext.Session);
            var items = cart.GetItems() ?? new List<CartItem>();

            // Populate Product objects when missing so view can render safely
            try
            {
                foreach (var it in items)
                {
                    if (it == null) continue;
                    if (it.Product == null || it.Product.Id == 0)
                    {
                        var p = _productService.GetById(it.ProductId);
                        if (p != null) it.Product = p;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log and continue — view will render placeholders for missing products
                _logger.LogWarning(ex, "Failed to populate product details for checkout view; rendering with available data.");
            }

            // If cart empty, redirect user back to cart (or home)
            if (items == null || !items.Any())
            {
                TempData["PaymentError"] = "Your cart is empty. Please add items before checking out.";
                return RedirectToAction("Index", "Cart");
            }

            // Compute totals defensively (product.Price may be null)
            decimal subtotal = items.Sum(i => (i.Product?.Price ?? 0m) * (i.Quantity));
            decimal delivery = 39.00m;
            decimal total = subtotal + delivery;

            ViewBag.Subtotal = subtotal;
            ViewBag.Delivery = delivery;
            ViewBag.Total = total;
            ViewBag.StripePublishable = _config["Stripe:PublishableKey"] ?? string.Empty;

            return View(items);
        }

        // Optional: simple simulated confirm (keeps previous behaviour)
        // Add to CheckoutController

        // GET: /Checkout/ConfirmQr
        [HttpGet]
        public IActionResult ConfirmQr()
        {
            // If someone opens the URL in browser (GET), redirect to Payment so they can scan/confirm properly
            return RedirectToAction("Payment");
        }

        // POST: /Checkout/ConfirmQr
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmQr(string payerName, string transactionRef = null)
        {
            var cart = new SessionCart(HttpContext.Session);
            var items = cart.GetItems();
            if (items == null || !items.Any())
            {
                TempData["PaymentError"] = "Your cart is empty. Please add items before confirming payment.";
                return RedirectToAction("Index", "Cart");
            }

            // Create order id and compute total
            var orderId = Guid.NewGuid().ToString().Split('-')[0].ToUpper();
            var total = items.Sum(i => (i.Product?.Price ?? 0m) * i.Quantity) + 39.00m;

            // TODO: persist order or log transactionRef/payerName if you need reconciliation

            // Clear the cart after confirmation
            cart.Clear();

            // Save some info for the ThankYou view
            TempData["PaymentMethod"] = "UPI-QR";
            TempData["PayerName"] = payerName;
            TempData["TransactionRef"] = transactionRef;
            TempData["OrderId"] = orderId;
            TempData["Total"] = total.ToString("0.00");

            return RedirectToAction("ThankYou", new { orderId = orderId, total = total });
        }
    }
}






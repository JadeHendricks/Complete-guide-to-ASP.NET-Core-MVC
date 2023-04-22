using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        [Area("customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

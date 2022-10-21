using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKi.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {
        public IActionResult GridLeftSidebar()
        {
            return View();
        }

        public IActionResult GridRightSidebar()
        {
            return View();
        }

        public IActionResult FullWidth()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKi.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

    }
}

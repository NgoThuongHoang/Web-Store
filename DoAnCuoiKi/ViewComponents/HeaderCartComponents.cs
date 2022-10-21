using DoAnCuoiKi.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace DoAnCuoiKi.ViewComponents
{
    public class HeaderCartComponents : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public HeaderCartComponents(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var giohang = _db.GioHang.ToList();
            return View(giohang);
        }
    }
}

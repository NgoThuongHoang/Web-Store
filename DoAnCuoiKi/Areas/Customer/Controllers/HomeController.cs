using DoAnCuoiKi.Data;
using DoAnCuoiKi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace DoAnCuoiKi.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        //Home/Index
        public IActionResult Index()
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.ToList();
            return View(sanpham);
        }

        //Home/GridLeftSidebar
        public IActionResult GridLeftSidebar()
        {
            return View();
        }

        //Details        
        public IActionResult Details(int sanphamId)
        {
            GioHang giohang = new GioHang()
            {
                SanPhamId = sanphamId,
                SanPham = _db.SanPham.Include(sp => sp.TheLoai).FirstOrDefault(sp => sp.Id == sanphamId),
                Quantity = 1                
            };
            return View(giohang);
        }

        [HttpPost]
        [Authorize] // Bat dang nhap
        public IActionResult Details(GioHang giohang)
        {
            //Lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            giohang.ApplicationUserId = claim.Value;

            //Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            var giohangdb = _db.GioHang.FirstOrDefault(sp => sp.SanPhamId == giohang.SanPhamId
            && sp.ApplicationUserId == giohang.ApplicationUserId);
            if (giohangdb == null)
            {
                _db.GioHang.Add(giohang);
            }
            else
            {
                giohangdb.Quantity += giohang.Quantity;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
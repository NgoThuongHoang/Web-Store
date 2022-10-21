using DoAnCuoiKi.Data;
using DoAnCuoiKi.Data.Migrations;
using DoAnCuoiKi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DoAnCuoiKi.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class GioHangController: Controller
    {
        private readonly ApplicationDbContext _db;
        public GioHangController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            //Lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            //Lay danh sach san pham trong gio hang cua User
            GioHangViewModel giohang = new GioHangViewModel()
            {
                DsGioHang = _db.GioHang
                .Include("SanPham")
                .Where(gh => gh.ApplicationUserId == claim.Value)
                .ToList(),
                HoaDon = new HoaDon()
            };
            foreach (var item in giohang.DsGioHang)
            {
                item.ProductPrice = item.Quantity * item.SanPham.Price;

                //++
                giohang.HoaDon.Total += item.ProductPrice;
            }
            return View(giohang);
        }

        //Giam so luong sp
        public IActionResult Giam(int giohangId)
        {
            //Lay thong tin gio hang
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);

            //Giam so luong sp -1
            giohang.Quantity -= 1;
            if (giohang.Quantity == 0)
            {
                _db.GioHang.Remove(giohang);
            }

            //Save
            _db.SaveChanges();
            //Ve gio hang
            return RedirectToAction("Index");
        }

        //Tang so luong sp
        public IActionResult Tang(int giohangId)
        {
            //Lay thong tin gio hang
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);

            //Giam so luong sp -1
            giohang.Quantity += 1;
            //Save
            _db.SaveChanges();
            //Ve gio hang
            return RedirectToAction("Index");
        }

        //Xoa sp
        public IActionResult Xoa(int giohangId)
        {
            //Lay thong tin gio hang
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);

            //Xoa
            _db.GioHang.Remove(giohang);
            //Save
            _db.SaveChanges();
            //Ve gio hang
            return RedirectToAction("Index");
        }

        //Thanh toan
        public IActionResult ThanhToan()
        {
            //Lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            //Lay danh sach san pham trong gio hang cua User
            GioHangViewModel giohang = new GioHangViewModel()
            {
                DsGioHang = _db.GioHang
                .Include("SanPham")
                .Where(gh => gh.ApplicationUserId == claim.Value)
                .ToList(),
                HoaDon = new HoaDon()
            };

            //Tim thong tin User trong CSDL de hien thi
            giohang.HoaDon.ApplicationUser = _db.ApplicationUser.FirstOrDefault(user => user.Id == claim.Value);

            //Gan du lieu vao hoa don
            giohang.HoaDon.Name = giohang.HoaDon.ApplicationUser.Name;
            giohang.HoaDon.PhoneNumber = giohang.HoaDon.ApplicationUser.PhoneNumber;
            giohang.HoaDon.Address = giohang.HoaDon.ApplicationUser.Address;

            foreach (var item in giohang.DsGioHang)
            {
                item.ProductPrice = item.Quantity * item.SanPham.Price;

                //++
                giohang.HoaDon.Total += item.ProductPrice;
            }
            return View(giohang);
        }

        //Thanh toan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThanhToan(GioHangViewModel giohang)
        {
            //Lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            //Cap nhap thong tin danh sach gio hang va hoa don
            giohang.DsGioHang = _db.GioHang.Include("SanPham")
                .Where(gh => gh.ApplicationUserId == claim.Value).ToList();
            giohang.HoaDon.ApplicationUserId = claim.Value;
            giohang.HoaDon.OrderDate = DateTime.Now;
            giohang.HoaDon.OrderStatus = "Đang xác nhận";

            foreach (var item in giohang.DsGioHang)
            {
                item.ProductPrice = item.Quantity * item.SanPham.Price;

                //++
                giohang.HoaDon.Total += item.ProductPrice;
            }
            _db.HoaDon.Add(giohang.HoaDon);
            _db.SaveChanges();

            // Thêm thông tin chi tiết hóa đơn 
            foreach (var item in giohang.DsGioHang.ToList())
            {

                ChiTietHoaDon chitiethoadon = new ChiTietHoaDon()
                {
                    SanPhamId = item.SanPhamId,
                    HoaDonId = giohang.HoaDon.Id,
                    ProductPrice = item.ProductPrice,
                    Quantity = item.Quantity
                };
                _db.ChiTietHoaDon.Add(chitiethoadon);
                _db.SaveChanges();
            }

            // Xóa thông tin trong giỏ hang
            _db.GioHang.RemoveRange(giohang.DsGioHang);
            _db.SaveChanges();
            // Quay về trang chủ
            return RedirectToAction("Index", "Home");
        }
    }
}



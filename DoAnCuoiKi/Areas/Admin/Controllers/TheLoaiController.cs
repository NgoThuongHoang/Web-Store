using DoAnCuoiKi.Data;
using DoAnCuoiKi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKi.Controllers
{
    [Area("Admin")]
    public class TheLoaiController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TheLoaiController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            /*ViewBag.HoTen = "Ngô Thượng Hoàng";
            ViewBag.Mssv = "162000700";
            ViewBag.Nam = "2020";*/
            /*            var theloai = new TheLoaiViewModel
                        {
                            Id = 1,
                            Name = "Trinh thám"
                        };

                        return View(theloai);
            */

            var theloai = _db.TheLoai.ToList();
            ViewBag.TheLoai = theloai;


            //Truy vấn tìm id > 3
            /*            var theloai = (from p in _db.TheLoai
                                       where p.Id > 3
                                       select p).ToList();
                        //_db.TheLoai.ToList();
                        ViewBag.TheLoai = theloai;*/


            //Truy vấn tìm thể loại tạo trước ngày 22/02/2022
            /*            var theloai = (from p in _db.TheLoai

                                       where (DateTime.Compare(p.DateCreated, DateTime.Parse("2022/02/02").Date)) < 0

                                       select p).ToList();

                        ViewBag.TheLoai = theloai;*/

            return View();
        }

        [HttpGet]
        //Buoi 4
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TheLoai theloai)
        {
            if (ModelState.IsValid)
            {
                _db.TheLoai.Add(theloai);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        //Edit
        [HttpGet]
        //Buoi 4
        public IActionResult Edit(int id)
        {
            /*            if (id == 0)
                        {
                            return NotFound();
                        }*/
            var theloai = _db.TheLoai.Find(id);
            return View(theloai);
        }

        [HttpPost]
        public IActionResult Edit(TheLoai theloai)
        {
            if (ModelState.IsValid)
            {
                _db.TheLoai.Update(theloai);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        //------------------------------------------------------------


        //Xoa
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var theloai = _db.TheLoai.Find(id);
            return View(theloai);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var theloai = _db.TheLoai.Find(id);
            if (theloai == null)
            {
                return NotFound();
            }
            _db.TheLoai.Remove(theloai);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult DetailsDonHang(int id)
        {
            var theloai = _db.TheLoai.Find(id);
            return View(theloai);
        }
    }
}

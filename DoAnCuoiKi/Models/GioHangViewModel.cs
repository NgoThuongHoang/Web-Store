namespace DoAnCuoiKi.Models
{
    public class GioHangViewModel
    {
        //Luu tru thong tin ds gio hang
        public IEnumerable<GioHang> DsGioHang { get; set; }

        //Them thuoc tinh tong tien 
        //public double Total { get; set; }
        public HoaDon HoaDon { get; set; }
    }
}

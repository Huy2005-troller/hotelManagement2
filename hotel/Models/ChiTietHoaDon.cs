namespace hotel.Models
{
    public class ChiTietHoaDon
    {
        public int Id { get; set; }
        public int HoaDonId { get; set; }

        public string TenDichVu { get; set; }
        public decimal DonGia { get; set; }

        public HoaDon HoaDon { get; set; }
    }

}

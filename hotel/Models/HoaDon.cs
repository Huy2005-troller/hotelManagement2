namespace hotel.Models
{
    public class HoaDon
    {
        public int Id { get; set; }
        public string TenKhach { get; set; }
        public string CCCD { get; set; }

        public int RoomId { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public decimal TongTien { get; set; }

        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public Room Room { get; set; }
    }

}

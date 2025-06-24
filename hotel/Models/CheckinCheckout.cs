namespace hotel.Models
{
    public class CheckinCheckout
    {
        public int Id { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public int RoomId { get; set; }
        public decimal Gia { get; set; }
        public string Status { get; set; }

        public Room Room { get; set; }
        public ICollection<CheckinCheckoutDetail> Details { get; set; }
    }

}

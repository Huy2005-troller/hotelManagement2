namespace hotel.Models
{
    public class CheckinCheckoutDetail
    {
        public int Id { get; set; }
        public int CheckinCheckoutId { get; set; }

        public string TenKhach { get; set; }
        public string CCCD { get; set; }

        public int? DichVuId { get; set; }
        public decimal? DonGiaDichVu { get; set; }

        public CheckinCheckout CheckinCheckout { get; set; }
        public DichVu? DichVu { get; set; }
    }

}

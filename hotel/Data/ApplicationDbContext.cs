using hotel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hotel.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<CheckinCheckout> CheckinCheckouts { get; set; }
        public DbSet<CheckinCheckoutDetail> CheckinCheckoutDetails { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }

    }
}

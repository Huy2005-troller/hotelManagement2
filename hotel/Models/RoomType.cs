using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
    public class RoomType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ⚠️ BẮT BUỘC có dòng này nếu Id tự tăng
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên loại phòng không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Loại phòng (Kind) không được để trống")]
        public string Kind { get; set; }  // ví dụ: "Luxury", "Standard"
        public ICollection<Room> Rooms { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
    public class RoomImage
    {
        [Key]
        public int Id { get; set; }

        public string Path { get; set; } // <-- Phải có dòng này!

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
public class Room
{
    [Key]
    public int IdRoom { get; set; }

    [Required(ErrorMessage = "Tên phòng không được bỏ trống")]
    public string NameRoom { get; set; }

    public string MoTa { get; set; }

    public string Status { get; set; }

    [Required(ErrorMessage = "Giá 1 không được bỏ trống")]
    [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public decimal Price1 { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi không được âm")]
    public decimal Price2 { get; set; }

    public List<RoomImage> Images { get; set; }

    [Required(ErrorMessage = "Bạn phải chọn loại phòng")]
    public int TypeRoomId { get; set; }

    [ForeignKey("TypeRoomId")]
    public RoomType TypeRoom { get; set; }
}

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace THWeb_Test.Models
{
    public class SinhVien
    {
        [Key]
        [StringLength(10)]
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        public string MaSV { get; set; }
        
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(50)]
        public string HoTen { get; set; }
        
        [StringLength(5)]
        public string GioiTinh { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }
        
        [StringLength(100)]
        public string? Hinh { get; set; }
        
        [StringLength(4)]
        public string MaNganh { get; set; }
        
        // Navigation properties
        [ForeignKey("MaNganh")]
        public virtual NganhHoc? NganhHoc { get; set; }
        
        public virtual ICollection<DangKy>? DangKys { get; set; }
    }
}
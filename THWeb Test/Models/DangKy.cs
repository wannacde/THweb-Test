using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace THWeb_Test.Models
{
    public class DangKy
    {
        [Key]
        public int MaDK { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? NgayDK { get; set; }
        
        [StringLength(10)]
        public string MaSV { get; set; }
        
        // Navigation properties
        [ForeignKey("MaSV")]
        public virtual SinhVien SinhVien { get; set; }
        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
    }
}
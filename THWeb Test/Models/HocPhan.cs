using System.ComponentModel.DataAnnotations;

namespace THWeb_Test.Models
{
    public class HocPhan
    {
        [Key]
        [StringLength(6)]
        public string MaHP { get; set; }
        
        [Required]
        [StringLength(30)]
        public string TenHP { get; set; }
        
        public int SoTinChi { get; set; }
        
        // Added for requirement 6: Expected number of students
        public int SoLuongDuKien { get; set; } = 30; // Default value
        
        // Navigation property
        public virtual ICollection<ChiTietDangKy> ChiTietDangKys { get; set; }
    }
}
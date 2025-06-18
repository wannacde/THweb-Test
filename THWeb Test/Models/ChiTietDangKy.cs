using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace THWeb_Test.Models
{
    public class ChiTietDangKy
    {
        public int MaDK { get; set; }
        
        [StringLength(6)]
        public string MaHP { get; set; }
        
        // Navigation properties
        [ForeignKey("MaDK")]
        public virtual DangKy DangKy { get; set; }
        
        [ForeignKey("MaHP")]
        public virtual HocPhan HocPhan { get; set; }
    }
}
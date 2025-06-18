using System.ComponentModel.DataAnnotations;

namespace THWeb_Test.Models
{
    public class NganhHoc
    {
        [Key]
        [StringLength(4)]
        public string MaNganh { get; set; }
        
        [StringLength(30)]
        public string TenNganh { get; set; }
        
        // Navigation property
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
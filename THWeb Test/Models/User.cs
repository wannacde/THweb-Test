using System.ComponentModel.DataAnnotations;

namespace THWeb_Test.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        
        [StringLength(10)]
        public string MaSV { get; set; }
    }
}
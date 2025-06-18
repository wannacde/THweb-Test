using System.ComponentModel.DataAnnotations;

namespace THWeb_Test.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
        [Display(Name = "Mã sinh viên")]
        [StringLength(10)]
        public string MaSV { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [Display(Name = "Mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
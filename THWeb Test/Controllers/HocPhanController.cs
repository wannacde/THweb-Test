using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using THWeb_Test.Models;
using System.Security.Claims;

namespace THWeb_Test.Controllers
{
    public class HocPhanController : Controller
    {
        private readonly Test1DbContext _context;

        public HocPhanController(Test1DbContext context)
        {
            _context = context;
        }

        // GET: HocPhan
        public async Task<IActionResult> Index()
        {
            var hocPhans = await _context.HocPhans.ToListAsync();
            return View(hocPhans);
        }

        // GET: HocPhan/DangKy
        [Authorize]
        public async Task<IActionResult> DangKy()
        {
            var hocPhans = await _context.HocPhans.Where(h => h.SoLuongDuKien > 0).ToListAsync();
            return View(hocPhans);
        }

        // POST: HocPhan/ThemVaoGio
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ThemVaoGio(string maHP)
        {
            // Get the current student ID from claims
            var maSV = User.FindFirstValue("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the course exists
            var hocPhan = await _context.HocPhans.FindAsync(maHP);
            if (hocPhan == null)
            {
                return NotFound();
            }

            // Check if there's space available
            if (hocPhan.SoLuongDuKien <= 0)
            {
                TempData["ErrorMessage"] = "Học phần đã đầy, không thể đăng ký thêm.";
                return RedirectToAction(nameof(DangKy));
            }

            // Get or create cart in session
            var cart = HttpContext.Session.Get<List<string>>("Cart") ?? new List<string>();
            
            // Add course to cart if not already there
            if (!cart.Contains(maHP))
            {
                cart.Add(maHP);
                HttpContext.Session.Set("Cart", cart);
                TempData["SuccessMessage"] = "Đã thêm học phần vào giỏ đăng ký.";
            }
            else
            {
                TempData["InfoMessage"] = "Học phần này đã có trong giỏ đăng ký.";
            }

            return RedirectToAction(nameof(GioDangKy));
        }

        // GET: HocPhan/GioDangKy
        [Authorize]
        public async Task<IActionResult> GioDangKy()
        {
            var cart = HttpContext.Session.Get<List<string>>("Cart") ?? new List<string>();
            var hocPhans = await _context.HocPhans.Where(h => cart.Contains(h.MaHP)).ToListAsync();
            return View(hocPhans);
        }

        // POST: HocPhan/XoaKhoiGio
        [HttpPost]
        [Authorize]
        public IActionResult XoaKhoiGio(string maHP)
        {
            var cart = HttpContext.Session.Get<List<string>>("Cart") ?? new List<string>();
            
            if (cart.Contains(maHP))
            {
                cart.Remove(maHP);
                HttpContext.Session.Set("Cart", cart);
                TempData["SuccessMessage"] = "Đã xóa học phần khỏi giỏ đăng ký.";
            }
            
            return RedirectToAction(nameof(GioDangKy));
        }

        // POST: HocPhan/XoaGio
        [HttpPost]
        [Authorize]
        public IActionResult XoaGio()
        {
            HttpContext.Session.Remove("Cart");
            TempData["SuccessMessage"] = "Đã xóa tất cả học phần khỏi giỏ đăng ký.";
            return RedirectToAction(nameof(GioDangKy));
        }

        // POST: HocPhan/LuuDangKy
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LuuDangKy()
        {
            var cart = HttpContext.Session.Get<List<string>>("Cart") ?? new List<string>();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Không có học phần nào trong giỏ đăng ký.";
                return RedirectToAction(nameof(GioDangKy));
            }

            var maSV = User.FindFirstValue("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("Login", "Account");
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create registration
                    var dangKy = new DangKy
                    {
                        NgayDK = DateTime.Now,
                        MaSV = maSV
                    };
                    _context.DangKys.Add(dangKy);
                    await _context.SaveChangesAsync();

                    // Add registration details
                    foreach (var maHP in cart)
                    {
                        var hocPhan = await _context.HocPhans.FindAsync(maHP);
                        if (hocPhan != null && hocPhan.SoLuongDuKien > 0)
                        {
                            var chiTiet = new ChiTietDangKy
                            {
                                MaDK = dangKy.MaDK,
                                MaHP = maHP
                            };
                            _context.ChiTietDangKys.Add(chiTiet);

                            // Decrease available slots
                            hocPhan.SoLuongDuKien--;
                            _context.HocPhans.Update(hocPhan);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // Clear cart
                    HttpContext.Session.Remove("Cart");
                    TempData["SuccessMessage"] = "Đăng ký học phần thành công!";
                    return RedirectToAction(nameof(DangKyThanhCong), new { id = dangKy.MaDK });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["ErrorMessage"] = $"Lỗi khi đăng ký: {ex.Message}";
                    return RedirectToAction(nameof(GioDangKy));
                }
            }
        }

        // GET: HocPhan/DangKyThanhCong/5
        [Authorize]
        public async Task<IActionResult> DangKyThanhCong(int id)
        {
            var dangKy = await _context.DangKys
                .Include(d => d.SinhVien)
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .FirstOrDefaultAsync(d => d.MaDK == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }

        // GET: HocPhan/DangKyCuaToi
        [Authorize]
        public async Task<IActionResult> DangKyCuaToi()
        {
            var maSV = User.FindFirstValue("MaSV");
            if (string.IsNullOrEmpty(maSV))
            {
                return RedirectToAction("Login", "Account");
            }

            var dangKys = await _context.DangKys
                .Include(d => d.ChiTietDangKys)
                .ThenInclude(c => c.HocPhan)
                .Where(d => d.MaSV == maSV)
                .OrderByDescending(d => d.NgayDK)
                .ToListAsync();

            return View(dangKys);
        }
    }
}
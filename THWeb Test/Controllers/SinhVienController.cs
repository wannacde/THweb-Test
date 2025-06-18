using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using THWeb_Test.Models;
using System.IO;

namespace THWeb_Test.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly Test1DbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<SinhVienController> _logger;

        public SinhVienController(Test1DbContext context, IWebHostEnvironment hostEnvironment, ILogger<SinhVienController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: SinhVien
        public async Task<IActionResult> Index()
        {
            var sinhViens = await _context.SinhViens.Include(s => s.NganhHoc).ToListAsync();
            return View(sinhViens);
        }

        // GET: SinhVien/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .Include(s => s.NganhHoc)
                .FirstOrDefaultAsync(m => m.MaSV == id);
                
            if (sinhVien == null)
            {
                return NotFound();
            }

            return View(sinhVien);
        }

        // GET: SinhVien/Create
        public IActionResult Create()
        {
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh");
            return View();
        }

        // POST: SinhVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SinhVien sinhVien, IFormFile imageFile)
        {
            // Clear existing model errors for navigation properties
            ModelState.Remove("NganhHoc");
            ModelState.Remove("DangKys");
            
            // Validate required fields manually
            if (string.IsNullOrEmpty(sinhVien.MaSV))
            {
                ModelState.AddModelError("MaSV", "Mã sinh viên là bắt buộc");
            }
            
            if (string.IsNullOrEmpty(sinhVien.HoTen))
            {
                ModelState.AddModelError("HoTen", "Họ tên là bắt buộc");
            }
            
            if (string.IsNullOrEmpty(sinhVien.MaNganh))
            {
                ModelState.AddModelError("MaNganh", "Ngành học là bắt buộc");
            }
            
            if (!ModelState.IsValid)
            {
                ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }

            try
            {
                // Handle file upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        // Create uploads directory if it doesn't exist
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate unique filename
                        string uniqueFileName = sinhVien.MaSV + "_" + Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        // Save filename to database (not full path)
                        sinhVien.Hinh = uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"File upload error: {ex.Message}");
                        ModelState.AddModelError("", "Error uploading file: " + ex.Message);
                        ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                        return View(sinhVien);
                    }
                }

                // Add to database
                _context.Add(sinhVien);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully added student with ID: {sinhVien.MaSV}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving student: {ex.Message}");
                ModelState.AddModelError("", "Error saving student: " + ex.Message);
                ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }
        }

        // GET: SinhVien/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
            return View(sinhVien);
        }

        // POST: SinhVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SinhVien sinhVien, IFormFile imageFile)
        {
            if (id != sinhVien.MaSV)
            {
                return NotFound();
            }

            // Clear existing model errors for navigation properties
            ModelState.Remove("NganhHoc");
            ModelState.Remove("DangKys");

            if (!ModelState.IsValid)
            {
                ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }

            try
            {
                // Handle file upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        // Create uploads directory if it doesn't exist
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate unique filename
                        string uniqueFileName = sinhVien.MaSV + "_" + Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        // Save filename to database (not full path)
                        sinhVien.Hinh = uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error uploading file: " + ex.Message);
                        ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                        return View(sinhVien);
                    }
                }

                try
                {
                    _context.Update(sinhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.MaSV))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating student: " + ex.Message);
                ViewData["MaNganh"] = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh", sinhVien.MaNganh);
                return View(sinhVien);
            }
        }

        // GET: SinhVien/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinhVien = await _context.SinhViens
                .Include(s => s.NganhHoc)
                .FirstOrDefaultAsync(m => m.MaSV == id);
                
            if (sinhVien == null)
            {
                return NotFound();
            }

            // Check if student has registrations
            var hasRegistrations = await _context.DangKys.AnyAsync(d => d.MaSV == id);
            if (hasRegistrations)
            {
                ViewBag.ErrorMessage = "Không thể xóa sinh viên này vì đã có đăng ký học phần.";
            }

            return View(sinhVien);
        }

        // POST: SinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // First, find all registrations for this student
                        var dangKys = await _context.DangKys.Where(d => d.MaSV == id).ToListAsync();
                        
                        // For each registration, delete the registration details first
                        foreach (var dangKy in dangKys)
                        {
                            var chiTietDangKys = await _context.ChiTietDangKys.Where(c => c.MaDK == dangKy.MaDK).ToListAsync();
                            _context.ChiTietDangKys.RemoveRange(chiTietDangKys);
                        }
                        
                        // Then delete the registrations
                        _context.DangKys.RemoveRange(dangKys);
                        
                        // Delete user account if exists
                        var userAccount = await _context.Users.FirstOrDefaultAsync(u => u.MaSV == id);
                        if (userAccount != null)
                        {
                            _context.Users.Remove(userAccount);
                        }
                        
                        // Now we can delete the student
                        var sinhVien = await _context.SinhViens.FindAsync(id);
                        if (sinhVien != null)
                        {
                            // Delete image file if exists
                            if (!string.IsNullOrEmpty(sinhVien.Hinh))
                            {
                                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", sinhVien.Hinh);
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }
                            
                            _context.SinhViens.Remove(sinhVien);
                        }
                        
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // If we get here, something went wrong with the delete
                TempData["ErrorMessage"] = "Không thể xóa sinh viên: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: SinhVien/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: SinhVien/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if student ID exists
                var sinhVien = await _context.SinhViens.FindAsync(model.MaSV);
                if (sinhVien == null)
                {
                    ModelState.AddModelError("MaSV", "Mã sinh viên không tồn tại trong hệ thống");
                    return View(model);
                }

                // Check if student already has an account
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.MaSV == model.MaSV);
                if (existingUser != null)
                {
                    ModelState.AddModelError("MaSV", "Mã sinh viên này đã được đăng ký tài khoản");
                    return View(model);
                }

                // Check if username already exists
                var usernameExists = await _context.Users.AnyAsync(u => u.Username == model.Username);
                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }

                // Create new user account
                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,
                    MaSV = model.MaSV
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công. Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        private bool SinhVienExists(string id)
        {
            return _context.SinhViens.Any(e => e.MaSV == id);
        }
    }
}
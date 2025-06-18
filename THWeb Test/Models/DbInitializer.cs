using Microsoft.EntityFrameworkCore;
using System;

namespace THWeb_Test.Models
{
    public static class DbInitializer
    {
        public static void Initialize(Test1DbContext context)
        {
            // Make sure the database exists
            context.Database.EnsureCreated();

            // Check if there's any data already
            if (context.NganhHocs.Any())
            {
                return; // DB has been seeded
            }

            // Add NganhHoc data
            var nganhHocs = new NganhHoc[]
            {
                new NganhHoc { MaNganh = "CNTT", TenNganh = "Công nghệ thông tin" },
                new NganhHoc { MaNganh = "QTKD", TenNganh = "Quản trị kinh doanh" }
            };
            context.NganhHocs.AddRange(nganhHocs);
            context.SaveChanges();

            // Add SinhVien data
            var sinhViens = new SinhVien[]
            {
                new SinhVien {
                    MaSV = "0123456789",
                    HoTen = "Nguyễn Văn A",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2000, 2, 12),
                    Hinh = "C:\\Users\\duy02\\OneDrive\\Pictures\\OIP.jpg",
                    MaNganh = "CNTT"
                },
                new SinhVien {
                    MaSV = "9876543210",
                    HoTen = "Nguyễn Thị B",
                    GioiTinh = "Nữ",
                    NgaySinh = new DateTime(2000, 7, 3),
                    Hinh = "C:\\Users\\duy02\\OneDrive\\Pictures\\OIP (2).jpg",
                    MaNganh = "QTKD"
                }
            };
            context.SinhViens.AddRange(sinhViens);
            context.SaveChanges();

            // Add HocPhan data
            var hocPhans = new HocPhan[]
            {
                new HocPhan { MaHP = "CNTT01", TenHP = "Lập trình C", SoTinChi = 3 },
                new HocPhan { MaHP = "CNTT02", TenHP = "Cơ sở dữ liệu", SoTinChi = 2 },
                new HocPhan { MaHP = "QTKD01", TenHP = "Kinh tế vi mô", SoTinChi = 2 },
                new HocPhan { MaHP = "QTDK02", TenHP = "Xác suất thống kê 1", SoTinChi = 3 }
            };
            context.HocPhans.AddRange(hocPhans);
            context.SaveChanges();
        }
    }
}
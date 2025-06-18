using Microsoft.EntityFrameworkCore;

namespace THWeb_Test.Models
{
    public class Test1DbContext : DbContext
    {
        public Test1DbContext(DbContextOptions<Test1DbContext> options) : base(options)
        {
        }

        public DbSet<NganhHoc> NganhHocs { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<HocPhan> HocPhans { get; set; }
        public DbSet<DangKy> DangKys { get; set; }
        public DbSet<ChiTietDangKy> ChiTietDangKys { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table names to match SQL script
            modelBuilder.Entity<NganhHoc>().ToTable("NganhHoc");
            modelBuilder.Entity<SinhVien>().ToTable("SinhVien");
            modelBuilder.Entity<HocPhan>().ToTable("HocPhan");
            modelBuilder.Entity<DangKy>().ToTable("DangKy");
            modelBuilder.Entity<ChiTietDangKy>().ToTable("ChiTietDangKy");
            modelBuilder.Entity<User>().ToTable("User");
            
            // Configure composite key for ChiTietDangKy
            modelBuilder.Entity<ChiTietDangKy>()
                .HasKey(c => new { c.MaDK, c.MaHP });
                
            // Configure relationships
            modelBuilder.Entity<SinhVien>()
                .HasOne(s => s.NganhHoc)
                .WithMany(n => n.SinhViens)
                .HasForeignKey(s => s.MaNganh);
                
            modelBuilder.Entity<DangKy>()
                .HasOne(d => d.SinhVien)
                .WithMany(s => s.DangKys)
                .HasForeignKey(d => d.MaSV);
                
            modelBuilder.Entity<ChiTietDangKy>()
                .HasOne(c => c.DangKy)
                .WithMany(d => d.ChiTietDangKys)
                .HasForeignKey(c => c.MaDK);
                
            modelBuilder.Entity<ChiTietDangKy>()
                .HasOne(c => c.HocPhan)
                .WithMany(h => h.ChiTietDangKys)
                .HasForeignKey(c => c.MaHP);
        }
    }
}
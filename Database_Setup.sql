-- Create database
CREATE DATABASE Test1
GO

USE Test1
GO

-- Create tables
CREATE TABLE NganhHoc
(
    MaNganh char(4) PRIMARY KEY,
    TenNganh nvarchar(30)
)
GO

CREATE TABLE SinhVien(
    MaSV char(10) PRIMARY KEY,
    HoTen nvarchar(50) NOT NULL,
    GioiTinh nvarchar(5),
    NgaySinh date,
    Hinh varchar(100),
    MaNganh char(4) REFERENCES NganhHoc(MaNganh)
)
GO

CREATE TABLE HocPhan
(
    MaHP char(6) PRIMARY KEY,
    TenHP nvarchar(30) NOT NULL,
    SoTinChi int
)
GO

CREATE TABLE DangKy(
    MaDK int IDENTITY(1,1) PRIMARY KEY,
    NgayDK date,
    MaSV char(10) REFERENCES SinhVien(MaSV)
)
GO

CREATE TABLE ChiTietDangKy(
    MaDK int REFERENCES DangKy(MaDK),
    MaHP char(6) REFERENCES HocPhan(MaHP),
    PRIMARY KEY(MaDK,MaHP)
)
GO

-- Insert data
INSERT INTO NganhHoc(MaNganh,TenNganh) VALUES('CNTT',N'Công nghệ thông tin')
INSERT INTO NganhHoc(MaNganh,TenNganh) VALUES('QTKD', N'Quản trị kinh doanh')
GO

-- Insert student data with updated image paths
INSERT INTO SinhVien(MaSV,HoTen,GioiTinh,NgaySinh,Hinh,MaNganh)
VALUES('0123456789',N'Nguyễn Văn A',N'Nam','12/02/2000','C:\Users\duy02\OneDrive\Pictures\OIP.jpg','CNTT')
INSERT INTO SinhVien(MaSV,HoTen,GioiTinh,NgaySinh,Hinh,MaNganh)
VALUES('9876543210',N'Nguyễn Thị B',N'Nữ','03/07/2000','C:\Users\duy02\OneDrive\Pictures\OIP (2).jpg','QTKD')
GO

-- Insert course data
INSERT INTO HocPhan(MaHP,TenHP,SoTinChi) VALUES('CNTT01',N'Lập trình C',3)
INSERT INTO HocPhan(MaHP,TenHP,SoTinChi) VALUES('CNTT02',N'Cơ sở dữ liệu',2)
INSERT INTO HocPhan(MaHP,TenHP,SoTinChi) VALUES('QTKD01',N'Kinh tế vi mô',2)
INSERT INTO HocPhan(MaHP,TenHP,SoTinChi) VALUES('QTDK02',N'Xác suất thống kê 1',3)
GO

-- Query to verify data
SELECT * FROM SinhVien
SELECT * FROM NganhHoc
SELECT * FROM HocPhan
SELECT * FROM DangKy
SELECT * FROM ChiTietDangKy


USE Test1
GO

-- Add SoLuongDuKien column to HocPhan table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HocPhan]') AND name = 'SoLuongDuKien')
BEGIN
    ALTER TABLE HocPhan ADD SoLuongDuKien INT NOT NULL DEFAULT 30
END
GO

-- Create User table for authentication
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
    CREATE TABLE [User] (
        UserId INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL,
        Password NVARCHAR(100) NOT NULL,
        MaSV CHAR(10) NULL REFERENCES SinhVien(MaSV)
    )
END
GO

-- Insert default users
IF NOT EXISTS (SELECT * FROM [User] WHERE Username = 'admin')
BEGIN
    INSERT INTO [User] (Username, Password) VALUES ('admin', 'admin123')
END

IF NOT EXISTS (SELECT * FROM [User] WHERE Username = 'student1')
BEGIN
    INSERT INTO [User] (Username, Password, MaSV) VALUES ('student1', 'student123', '0123456789')
END

IF NOT EXISTS (SELECT * FROM [User] WHERE Username = 'student2')
BEGIN
    INSERT INTO [User] (Username, Password, MaSV) VALUES ('student2', 'student123', '9876543210')
END
GO

-- Update HocPhan with SoLuongDuKien values
UPDATE HocPhan SET SoLuongDuKien = 30 WHERE MaHP = 'CNTT01'
UPDATE HocPhan SET SoLuongDuKien = 25 WHERE MaHP = 'CNTT02'
UPDATE HocPhan SET SoLuongDuKien = 40 WHERE MaHP = 'QTKD01'
UPDATE HocPhan SET SoLuongDuKien = 35 WHERE MaHP = 'QTDK02'
GO





 DECLARE @Username NVARCHAR(50) = 'student1';
 DECLARE @Password NVARCHAR(100) = 'student123';
SELECT u.UserId, u.Username, u.MaSV, s.HoTen, s.MaNganh, n.TenNganh
FROM [User] u
LEFT JOIN SinhVien s ON u.MaSV = s.MaSV
LEFT JOIN NganhHoc n ON s.MaNganh = n.MaNganh
WHERE u.Username = @Username AND u.Password = @Password;



select * from HocPhan
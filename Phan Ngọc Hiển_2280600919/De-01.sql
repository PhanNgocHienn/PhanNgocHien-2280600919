CREATE DATABASE QuanLySV;
USE QuanLySV;

CREATE TABLE Lop (
    MaLop CHAR(3) PRIMARY KEY,
    TenLop NVARCHAR(30) NOT NULL
);

CREATE TABLE Sinhvien (
    MaSV CHAR(6) PRIMARY KEY,
    HoTenSV NVARCHAR(40),
    MaLop CHAR(3),
    NgaySinh DATETIME,
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);

INSERT INTO Lop (MaLop, TenLop)
VALUES ('L01', N'Công nghệ thông tin'), 
       ('L02', N'Khoa học máy tính');

	   INSERT INTO Sinhvien (MaSV, HoTenSV, MaLop, NgaySinh)
VALUES ('SV001', 'Nguyen Van A', 'L01', '2001-01-15'),
       ('SV002', 'Tran Thi B', 'L01', '2000-12-10'),
       ('SV003', 'Le Van C', 'L02', '1999-05-25'),
       ('SV004', 'Pham Thi D', 'L02', '2002-03-30');

	   SELECT * FROM Lop;
SELECT * FROM Sinhvien;


DROP DATABASE QuanLyPhongKham;
CREATE DATABASE IF NOT EXISTS QuanLyPhongKham;
USE QuanLyPhongKham;

CREATE TABLE LOAIPHONGKHAM (
    MaLoaiPhongKham VARCHAR(20) PRIMARY KEY,
    TenLoaiPhongKham VARCHAR(100) NOT NULL,
    SoLuongToiDa INT NOT NULL
);

CREATE TABLE BENHNHAN (
    MaBenhNhan VARCHAR(20) PRIMARY KEY,
    HoTen VARCHAR(100) NOT NULL,
    GioiTinh VARCHAR(10),
    NamSinh INT,
    DiaChi VARCHAR(255)
);

CREATE TABLE KHAMBENH (
    MaKhamBenh VARCHAR(20) PRIMARY KEY,
    NgayKham DATE NOT NULL,
    MaLoaiPhongKham VARCHAR(20),
    FOREIGN KEY (MaLoaiPhongKham) REFERENCES LOAIPHONGKHAM(MaLoaiPhongKham)
);

CREATE TABLE CHITIETKHAMBENH (
    MaKhamBenh VARCHAR(20),
    MaBenhNhan VARCHAR(20),
    PRIMARY KEY (MaKhamBenh, MaBenhNhan),
    FOREIGN KEY (MaKhamBenh) REFERENCES KHAMBENH(MaKhamBenh),
    FOREIGN KEY (MaBenhNhan) REFERENCES BENHNHAN(MaBenhNhan)
);

INSERT INTO LOAIPHONGKHAM (MaLoaiPhongKham, TenLoaiPhongKham, SoLuongToiDa) VALUES 
('LPK01', 'Phòng khám Thường', 40),
('LPK02', 'Phòng khám VIP', 20);

INSERT INTO BENHNHAN (MaBenhNhan, HoTen, GioiTinh, NamSinh, DiaChi) VALUES 
('BN01', 'Nguyễn Văn A', 'Nam', 1985, 'Hà Nội'),
('BN02', 'Trần Thị B', 'Nữ', 1990, 'TP.HCM'),
('BN03', 'Lê Văn C', 'Nam', 1978, 'Đà Nẵng'),
('BN04', 'Phạm Thị D', 'Nữ', 2005, 'Hải Phòng'),
('BN05', 'Hoàng Văn E', 'Nam', 1992, 'Cần Thơ'),
('BN06', 'Vũ Thị F', 'Nữ', 1988, 'Huế'),
('BN07', 'Đặng Văn G', 'Nam', 1965, 'Nam Định'),
('BN08', 'Bùi Thị H', 'Nữ', 1975, 'Thanh Hóa'),
('BN09', 'Đỗ Văn I', 'Nam', 1999, 'Ninh Bình'),
('BN10', 'Ngô Thị K', 'Nữ', 1982, 'Vĩnh Phúc'),
('BN11', 'Dương Văn L', 'Nam', 2010, 'Bắc Ninh'),
('BN12', 'Lý Thị M', 'Nữ', 1955, 'Thái Nguyên'),
('BN13', 'Đinh Văn N', 'Nam', 1995, 'Quảng Ninh'),
('BN14', 'Phan Thị O', 'Nữ', 1980, 'Gia Lai'),
('BN15', 'Mai Văn P', 'Nam', 1970, 'Đắk Lắk');

INSERT INTO KHAMBENH (MaKhamBenh, NgayKham, MaLoaiPhongKham) VALUES 
('KB01', '2026-03-16', 'LPK01'),
('KB02', '2026-03-16', 'LPK02'),
('KB03', '2026-03-15', 'LPK01'),
('KB04', '2026-03-15', 'LPK02'),
('KB05', '2026-03-14', 'LPK01'),
('KB06', '2026-03-13', 'LPK02'),
('KB07', '2026-03-12', 'LPK01'),
('KB08', '2026-03-12', 'LPK02'),
('KB09', '2026-03-11', 'LPK01'),
('KB10', '2026-03-11', 'LPK02');

INSERT INTO CHITIETKHAMBENH (MaKhamBenh, MaBenhNhan) VALUES 
('KB01', 'BN01'),
('KB01', 'BN03'),
('KB01', 'BN05'),
('KB01', 'BN07'),
('KB01', 'BN09'),
('KB01', 'BN11'),
('KB01', 'BN13'),
('KB02', 'BN02'),
('KB02', 'BN04'),
('KB02', 'BN06'),
('KB02', 'BN08'),
('KB02', 'BN10'),
('KB03', 'BN01'),
('KB03', 'BN12'),
('KB03', 'BN14'),
('KB03', 'BN15'),
('KB04', 'BN02'),
('KB04', 'BN05'),
('KB04', 'BN09'),
('KB04', 'BN13'),
('KB05', 'BN03'),
('KB05', 'BN07'),
('KB05', 'BN11'),
('KB05', 'BN14'),
('KB06', 'BN04'),
('KB06', 'BN08'),
('KB06', 'BN10'),
('KB06', 'BN12'),
('KB06', 'BN15'),
('KB07', 'BN01'),
('KB07', 'BN03'),
('KB07', 'BN05'),
('KB07', 'BN09'),
('KB07', 'BN13'),
('KB08', 'BN02'),
('KB08', 'BN06'),
('KB08', 'BN11'),
('KB08', 'BN14'),
('KB09', 'BN01'),
('KB09', 'BN02'),
('KB09', 'BN03'),
('KB09', 'BN04'),
('KB09', 'BN05'),
('KB10', 'BN01'),
('KB10', 'BN10'),
('KB10', 'BN12'),
('KB10', 'BN14'),
('KB10', 'BN15');
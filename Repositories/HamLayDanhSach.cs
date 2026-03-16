using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using QuanLyPhongKham_Final.Models;
using System.Windows; // Để dùng MessageBox hiện lỗi

namespace QuanLyPhongKham_Final.Repositories
{
    public class HamLayDanhSach
    {
        private readonly string connectionString = "server=localhost;port=3306;user=root;password=123456789;database=QuanLyPhongKham;";

        // 1. Lấy danh sách Loại Phòng Khám (Đổ vào ComboBox)
        public List<LoaiPhongKham> LayDanhSachLoaiPhong()
        {
            List<LoaiPhongKham> danhSach = new List<LoaiPhongKham>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Tên cột phải khớp chính xác với sơ đồ ERD
                    string query = "SELECT MaLoaiPhongKham, TenLoaiPhongKham, SoLuongToiDa FROM LOAIPHONGKHAM";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSach.Add(new LoaiPhongKham
                                {
                                    MaLoaiPhongKham = reader.GetString("MaLoaiPhongKham"),
                                    TenLoaiPhongKham = reader.GetString("TenLoaiPhongKham"),
                                    SoLuongToiDa = reader.GetInt32("SoLuongToiDa")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lấy Loại Phòng: " + ex.Message); }
            return danhSach;
        }

        // 2. Lấy Bệnh Nhân theo Ngày và Loại Phòng (Dựa trên sơ đồ JOIN 3 bảng)
        public List<BenhNhan> LayBenhNhanTheoDieuKien(DateTime ngayKham, string maLoaiPhong)
        {
            List<BenhNhan> danhSach = new List<BenhNhan>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // JOIN theo đúng sơ đồ: BENHNHAN -> CHITIETKHAMBENH -> KHAMBENH
                    string query = @"
                        SELECT bn.MaBenhNhan, bn.HoTen, bn.GioiTinh, bn.NamSinh, bn.DiaChi
                        FROM BENHNHAN bn
                        JOIN CHITIETKHAMBENH ct ON bn.MaBenhNhan = ct.MaBenhNhan
                        JOIN KHAMBENH kb ON ct.MaKhamBenh = kb.MaKhamBenh
                        WHERE kb.NgayKham = @NgayKham AND kb.MaLoaiPhongKham = @MaLoaiPhong";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NgayKham", ngayKham.Date);
                        cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSach.Add(new BenhNhan
                                {
                                    MaBenhNhan = reader.GetString("MaBenhNhan"),
                                    HoTen = reader.GetString("HoTen"),
                                    GioiTinh = reader.IsDBNull(2) ? "" : reader.GetString("GioiTinh"),
                                    NamSinh = reader.IsDBNull(3) ? 0 : reader.GetInt32("NamSinh"),
                                    DiaChi = reader.IsDBNull(4) ? "" : reader.GetString("DiaChi")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi lấy Bệnh Nhân: " + ex.Message); }
            return danhSach;
        }
    }
}
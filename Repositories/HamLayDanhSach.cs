using MySql.Data.MySqlClient;
using QuanLyPhongKham_Final.DBHelper;
using System;
using System.Collections.Generic;
using System.Windows;
using QuanLyPhongKham_Final.Models;

namespace QuanLyPhongKham_Final.Repositories
{
    public class HamLayDanhSach
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        // 1. Lấy danh sách Loại Phòng Khám
        public List<LoaiPhongKham> LayDanhSachLoaiPhong()
        {
            List<LoaiPhongKham> danhSach = new List<LoaiPhongKham>();
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
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

        // 2. Lấy Bệnh Nhân theo Ngày và Loại Phòng
        public List<BenhNhan> LayBenhNhanTheoDieuKien(DateTime ngayKham, string maLoaiPhong)
        {
            List<BenhNhan> danhSach = new List<BenhNhan>();
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
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
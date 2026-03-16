using MySql.Data.MySqlClient;
using QuanLyPhongKham_Final.DBHelper;
using QuanLyPhongKham_Final.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace QuanLyPhongKham_Final.Repositories
{
    public class RepoBenhNhan
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        /// <summary>
        /// Lấy danh sách tất cả bệnh nhân
        /// </summary>
        public List<BenhNhan> LayDanhSachAll()
        {
            List<BenhNhan> danhSach = new List<BenhNhan>();
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MaBenhNhan, HoTen, GioiTinh, NamSinh, DiaChi FROM BENHNHAN ORDER BY HoTen";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi lấy danh sách bệnh nhân: " + ex.Message);
            }
            return danhSach;
        }

        /// <summary>
        /// Lấy bệnh nhân theo ngày khám và loại phòng
        /// </summary>
        public List<BenhNhan> LayTheoDieuKien(DateTime ngayKham, string maLoaiPhong)
        {
            List<BenhNhan> danhSach = new List<BenhNhan>();
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT DISTINCT bn.MaBenhNhan, bn.HoTen, bn.GioiTinh, bn.NamSinh, bn.DiaChi
                        FROM BENHNHAN bn
                        JOIN CHITIETKHAMBENH ct ON bn.MaBenhNhan = ct.MaBenhNhan
                        JOIN KHAMBENH kb ON ct.MaKhamBenh = kb.MaKhamBenh
                        WHERE kb.NgayKham = @NgayKham AND kb.MaLoaiPhongKham = @MaLoaiPhong
                        ORDER BY bn.HoTen";

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
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi lấy bệnh nhân theo điều kiện: " + ex.Message);
            }
            return danhSach;
        }
    }
}
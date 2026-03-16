using MySql.Data.MySqlClient;
using QuanLyPhongKham_Final.DBHelper;
using QuanLyPhongKham_Final.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace QuanLyPhongKham_Final.Repositories
{
    public class RepoLoaiPhongKham
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        /// <summary>
        /// Lấy danh sách tất cả loại phòng khám
        /// </summary>
        public List<LoaiPhongKham> LayDanhSachAll()
        {
            List<LoaiPhongKham> danhSach = new List<LoaiPhongKham>();
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MaLoaiPhongKham, TenLoaiPhongKham, SoLuongToiDa FROM LOAIPHONGKHAM ORDER BY TenLoaiPhongKham";
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
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi lấy danh sách loại phòng khám: " + ex.Message);
            }
            return danhSach;
        }
    }
}
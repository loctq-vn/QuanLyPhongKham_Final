using MySql.Data.MySqlClient;
using QuanLyPhongKham_Final.DBHelper;
using System;
using System.Windows;

namespace QuanLyPhongKham_Final.Repositories
{
    public class RepoChiTietKhamBenh
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        /// <summary>
        /// Thêm chi tiết khám bệnh (liên kết bệnh nhân với phiếu khám)
        /// </summary>
        public bool Them(string maKhamBenh, string maBenhNhan)
        {
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO CHITIETKHAMBENH (MaKhamBenh, MaBenhNhan) 
                        VALUES (@MaKhamBenh, @MaBenhNhan)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhamBenh", maKhamBenh);
                        cmd.Parameters.AddWithValue("@MaBenhNhan", maBenhNhan);
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm chi tiết khám bệnh: " + ex.Message);
                return false;
            }
        }
    }
}
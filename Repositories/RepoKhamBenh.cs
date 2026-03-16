using MySql.Data.MySqlClient;
using QuanLyPhongKham_Final.DBHelper;
using System;
using System.Windows;

namespace QuanLyPhongKham_Final.Repositories
{
    public class RepoKhamBenh
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        /// <summary>
        /// Thêm mới phiếu khám bệnh
        /// </summary>
        public string Them(DateTime ngayKham, string maLoaiPhong)
        {
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();

                    // Tạo mã khám bệnh mới
                    string maKhamBenh = "KB" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    string query = @"
                        INSERT INTO KHAMBENH (MaKhamBenh, NgayKham, MaLoaiPhongKham) 
                        VALUES (@MaKhamBenh, @NgayKham, @MaLoaiPhong)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhamBenh", maKhamBenh);
                        cmd.Parameters.AddWithValue("@NgayKham", ngayKham.Date);
                        cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);
                        cmd.ExecuteNonQuery();
                    }

                    return maKhamBenh;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm phiếu khám bệnh: " + ex.Message);
                return null;
            }
        }
    }
}
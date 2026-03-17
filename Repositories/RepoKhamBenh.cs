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

        // Thêm hàm này vào trong class RepoKhamBenh của bạn (giữ nguyên các hàm cũ)
        public int LaySoLuongBenhNhanTrongNgay(DateTime ngayKham, string maLoaiPhong)
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = dbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                    SELECT COUNT(*) 
                    FROM CHITIETKHAMBENH ctkb
                    INNER JOIN KHAMBENH kb ON ctkb.MaKhamBenh = kb.MaKhamBenh
                    WHERE kb.MaLoaiPhongKham = @MaLoaiPhong
                    AND kb.NgayKham = @NgayKham";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLoaiPhong", maLoaiPhong);
                        cmd.Parameters.AddWithValue("@NgayKham", ngayKham.ToString("yyyy-MM-dd"));

                        // ExecuteScalar dùng để lấy về 1 giá trị duy nhất (kết quả của COUNT)
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi đếm số lượng bệnh nhân: " + ex.Message);
            }
            return count;
        }
    }

}
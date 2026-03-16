using MySql.Data.MySqlClient;

namespace QuanLyPhongKham_Final.DBHelper
{
    public class DatabaseHelper
    {
        // Thay thế root và password bằng thông tin MySQL của bạn
        private readonly string connectionString = "Server=localhost;Database=QuanLyPhongKham;Uid=root;Pwd=123456789;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
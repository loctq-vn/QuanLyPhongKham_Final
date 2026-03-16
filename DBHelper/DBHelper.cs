using MySql.Data.MySqlClient;

namespace QuanLyPhongKham_Final.DBHelper
{
    public class DatabaseHelper
    {
        // Thay thế root và password bằng thông tin MySQL của bạn
        private readonly string connectionString =
            "Server=127.0.0.1;Port=3306;Database=quanlyphongkham;Uid=root;Pwd=123456;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}